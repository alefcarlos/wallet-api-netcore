using System;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Wallet.Infra.Data.Repositories
{
    using Domain.Models;
    using Domain.Interfaces.Repositories;
    using Domain.Core.Exceptions;

    public class WalletUserRepository : RepositoryBase<WalletUser>, IWalletUserRepository
    {
        public WalletUserRepository(WalletContext context, ILogger<WalletUserRepository> logger)
            : base(context, logger)
        {
        }

        public override void BeforeAdd(WalletUser entity)
        {
            entity.Code = Guid.NewGuid();

            base.BeforeAdd(entity);
        }

        /// <summary>
        /// Gets the wallet informar from an user 
        /// </summary>
        /// <param name="userId">UserId requested</param>
        /// <returns>Returns cards and transactions from wallet</returns>
        public async Task<WalletUser> GetInfoAsync(int userId)
        {
            var entity = await Query()
                            .Include(card => card.Cards)
                                .ThenInclude(t=> t.Transactions)
                            .FirstOrDefaultAsync(x=>x.WalletUserId == userId);

            if (entity == null)
                throw new RecordNotFoundException();

            return entity;
        }
    }
}