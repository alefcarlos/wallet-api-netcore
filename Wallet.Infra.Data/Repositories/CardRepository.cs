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


    public class CardRepository : RepositoryBase<Card>, ICardRepository
    {
        public CardRepository(WalletContext context, ILogger<CardRepository> logger)
            : base(context, logger)
        {
        }

        /// <summary>
        /// Gets all cards by user
        /// </summary>
        /// <param name="id">WallerUserId</param>
        /// <returns>Returns a card list.</returns>
        public async Task<List<Card>> GetByUserId(int id)
        {
            var result = await Query().Where(x => x.WalletUserId == id).ToListAsync();

            return result;
        }

        public override void BeforeAdd(Card entity)
        {
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