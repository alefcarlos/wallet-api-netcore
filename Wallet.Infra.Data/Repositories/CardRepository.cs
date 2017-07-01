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

    public class CardRepository : RepositoryBase<Card>, ICardRepository
    {
        //Allow us to handle the logged user 
        private readonly IUserManagment _userManagment;

        public CardRepository(WalletContext context,
                            ILogger<CardRepository> logger,
                            IUserManagment userManagment)

            : base(context, logger)
        {
            _userManagment = userManagment;
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

            ApplyCommomValidations(entity);

            //We can    
            base.BeforeAdd(entity);
        }


        public override void BeforeUpdate(Card entity)
        {
            ApplyCommomValidations(entity);

            //We can    
            base.BeforeUpdate(entity);
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

        public async Task<List<Card>> GetAllAvailableLimitAsync()
        {
            return await Query().Where(c => c.Limit >= 0)
                                .AsNoTracking()
                                .OrderByDescending(x => x.DueDate)
                                .ThenBy(x => x.Limit)
                                .ToListAsync();
        }
    }
}