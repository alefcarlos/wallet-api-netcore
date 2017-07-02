using System;
using Microsoft.Extensions.Logging;

namespace Wallet.Infra.Data.Repositories
{
    using Domain.Models;
    using Domain.Interfaces.Repositories;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using Wallet.Domain.Core.Exceptions;
    using Wallet.Domain.Interfaces.User;

    public class CardTransactionRepository : RepositoryBase<CardTransaction>,
                                                ICardTrasactionRepository
    {
        private readonly ICardRepository _cardRepo;
        private readonly IWalletUserRepository _userRepo;



        public CardTransactionRepository(WalletContext context,
                                            ILogger<CardTransactionRepository> logger,
                                            ICardRepository cardRepo,
                                            IWalletUserRepository userRepo,
                                            IUserManagment userManagment)
            : base(context, logger, userManagment)
        {
            _cardRepo = cardRepo;
            _userRepo = userRepo;
        }

        public override void BeforeAdd(CardTransaction entity)
        {
            entity.Date = DateTime.Now;

            ApplyCommomValidations(entity);

            //We can    
            base.BeforeAdd(entity);
        }

        public override void AfterAdd(CardTransaction entity)
        {
            //Subtracting the card limit
            _cardRepo.SubtractLimit(entity.CardId, entity.Value);
        }

        private void ApplyCommomValidations(CardTransaction entity)
        {
            //The value must be greater than 0
            if (entity.Value == 0)
                throw new Exception("You must inform a transaction value!");
        }

        /// <summary>
        /// Gets all transactions by card
        /// </summary>
        /// <returns>Returns a transaction list.</returns>
        public async Task<IEnumerable<CardTransaction>> GetAllByCardIdAsync(int cardId)
        {
            return await Query().Where(x => x.CardId == cardId).ToListAsync();
        }

        /// <summary>
        /// Adds a new card transaction
        /// </summary>
        /// <param name="entity">Transactions info</param>
        /// <returns></returns>
        public async Task AddNewTransactionAsync(CardTransaction entity)
        {
            switch (entity.Type)
            {
                case ECardTransactionType.Purchase:
                    await AddPurchaseTransaction(entity);
                    return;

                case ECardTransactionType.ReleaseCredit:
                    return;
            }
        }

        /// <summary>
        /// Create transactios based on value.
        /// </summary>
        /// <param name="entity">Transaction values</param>
        private async Task AddPurchaseTransaction(CardTransaction entity)
        {
            //Get user info
            var userEntity = await _userRepo.GetAsync(_userManagment.User.WalletUserId);

            //If the value is greater than real limit
            if (userEntity.RealLimit > 0 && userEntity.RealLimit < entity.Value)
                throw new ThereIsNoEnoughLimit();

            //Get all cards with limit
            var allCards = await _cardRepo.GetAllAvailableLimitAsync();

            //There is no card available
            if (!allCards.Any())
                throw new ThereIsNoCardAvailableException();

            var entities = PickBestCards(allCards, entity);

            try
            {
                dataContext.Database.BeginTransaction();

                foreach (var item in entities)
                {
                    await AddAsync(item);
                }

                dataContext.Database.CommitTransaction();
            }
            catch
            {
                dataContext.Database.RollbackTransaction();
            }
        }


        /// <summary>
        /// This method is used to select the bests cards based on transaction value and user available limits
        /// </summary>
        /// <param name="availableCards">Card list with limit > 0</param>
        /// <param name="entity">Transaction infos</param>
        /// <returns>Returns all transaction created for this value</returns>
        private List<CardTransaction> PickBestCards(List<Card> availableCards, CardTransaction entity)
        {
            var result = new List<CardTransaction>();
            var _totalValue = entity.Value;
            var _remaingValue = 0m;
            var _transactionValue = 0m;

            if (availableCards.Sum(x => x.AvailableLimit) < _totalValue)
                throw new ThereIsNoEnoughLimit();

            //If there is just a card and this limit is ok
            var _current = availableCards.First();
            if (availableCards.Count == 1)
            {
                result.Add(new CardTransaction
                {
                    CardId = _current.CardId,
                    Description = entity.Description,
                    Value = entity.Value,
                    Type = ECardTransactionType.Purchase
                });

                goto returnResult;
            }

            // Iterating until de total value becomes 0.
            foreach (var card in availableCards)
            {
                _transactionValue = _totalValue <= card.Limit ? _totalValue : card.Limit;
                _remaingValue = _totalValue - _transactionValue;

                result.Add(new CardTransaction
                {
                    CardId = card.CardId,
                    Description = entity.Description,
                    Value = _transactionValue,
                    Type = ECardTransactionType.Purchase
                });

                _totalValue = _remaingValue;

                //We've already generated all transactions, so _remaingValue = _totalValue = 0 ;
                if (_remaingValue <= 0)
                    break;
            }

            returnResult:
            return result;
        }
    }
}