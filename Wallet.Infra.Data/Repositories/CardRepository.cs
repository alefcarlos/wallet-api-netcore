using System;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Wallet.Infra.Data.Repositories
{
    using Domain.Models;
    using Domain.Interfaces.Repositories;
    using Wallet.Domain.Interfaces.User;
    using Wallet.Domain.Core.Exceptions;

    public class CardRepository : RepositoryBase<Card>, ICardRepository
    {
        public CardRepository(WalletContext context,
                            ILogger<CardRepository> logger,
                            IUserManagment userManagment)

            : base(context, logger, userManagment)
        {
        }

        /// <summary>
        /// Gets all cards by logged user
        /// </summary>
        /// <returns>Returns a card list.</returns>
        public async Task<List<Card>> GetByLoggedUser()
        {
            var result = await Query()
                                .Where(x => x.WalletUserId == _userManagment.User.WalletUserId)
                                .ToListAsync();

            return result;
        }

        /// <summary>
        /// Gets the cards's total limit 
        /// </summary>
        /// <returns>Return the total limit for this user</returns>
        public async Task<decimal> GetSumLimit() => await Query().SumAsync(x => x.Limit);

        public override void BeforeAdd(Card entity)
        {
            //Adds owner user.
            entity.WalletUserId = _userManagment.User.WalletUserId;

            //Available limit is equals to Limit
            entity.AvailableLimit = entity.Limit; 

            ApplyCommomValidations(entity);
    
            base.BeforeAdd(entity);
        }

        public override void BeforeUpdate(Card entity)
        {
            ApplyCommomValidations(entity);
 
            base.BeforeUpdate(entity);
        }

        public override void BeforeDelete(Card entity)
        {
            //Validate if the owner from this card is the logged user
            if(entity.WalletUserId != _userManagment.User.WalletUserId)
                throw new NotAllowedException();
        }

        private void ApplyCommomValidations(Card entity)
        {
            //The due must be valid
            if (!(entity.DueDate >= 1 && entity.DueDate <= 28))
                throw new Exception($"The due date must be between 1st and 28th.");

            //The card must have a valid ExpirationDate
            if (entity.ExpirationDate <= DateTime.Today)
                throw new Exception("The expiration date must be greater than today!");
        }

        /// <summary>
        /// Gets all user's cards with limit > 0
        /// </summary>
        /// <returns>Return a card list</returns>
        public async Task<List<Card>> GetAllAvailableLimitAsync()
        {
            return await Query().Where(c => c.Limit >= 0)
                                .AsNoTracking()
                                .OrderByDescending(x => x.DueDate)
                                .ThenBy(x => x.Limit)
                                .ToListAsync();
        }

        /// <summary>
        /// Subtract a value from available card limit.
        /// </summary>
        /// <param name="cardId">Card id</param>
        /// <param name="value">Value to subtract</param>
        public void SubtractLimit(int cardId, decimal value)
        {
            var sql = "UPDATE Cards SET AvailableLimit = (AvailableLimit - {0}) WHERE CardId = {1}";

            ExecuteQuery(sql, value, cardId);
        }
    }
}