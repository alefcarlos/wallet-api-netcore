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

    public class CardTransactionRepository : RepositoryBase<CardTransaction>,
                                                ICardTrasactionRepository
    {
        private readonly ICardRepository _cardRepo;
        public CardTransactionRepository(WalletContext context,
                                            ILogger<CardTransactionRepository> logger,
                                            ICardRepository cardRepo)
            : base(context, logger)
        {
            _cardRepo = cardRepo;
        }

        public override void BeforeAdd(CardTransaction entity)
        {
            entity.Date = DateTime.Now;

            ApplyCommomValidations(entity);

            //We can    
            base.BeforeAdd(entity);
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

        public async Task<CardTransaction> AddNewTransactionAsync(CardTransaction entity)
        {
            //Get all cards with limit
            var allCards = await _cardRepo.GetAllAvailableLimitAsync();

            //There is no card available
            if (!allCards.Any())
                throw new ThereIsNoCardAvailableException();

            var entities = GenerateTransactions(allCards, entity);

            throw new NotImplementedException();
        }

        private object GenerateTransactions(List<Card> availableCards, CardTransaction entity)
        {
            var transactions = PickBestCards(availableCards, entity);

            //There is no card available
            if (!transactions.Any())
                throw new ThereIsNoCardAvailableException();

            return transactions;
        }
        private List<CardTransaction> PickBestCards(List<Card> availableCards, CardTransaction entity)
        {
            var result = new List<CardTransaction>();
            var _totalValue = entity.Value;
            var _remaingValue = 0m;
            var _transactionValue = 0m;

            if (availableCards.Sum(x => x.Limit) < _totalValue)
                throw new ThereIsNoLimitEnough();

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


            // Iterate until de total value becomes 0.
            foreach (var card in availableCards)
            {
                _transactionValue = _totalValue <= card.Limit ? _totalValue : card.Limit;
                _remaingValue = _totalValue -  _transactionValue;

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