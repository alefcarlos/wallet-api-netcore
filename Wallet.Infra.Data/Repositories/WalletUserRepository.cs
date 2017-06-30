using System;
using Microsoft.Extensions.Logging;

namespace Wallet.Infra.Data.Repositories
{
    using Domain.Models;
    using Domain.Interfaces.Repositories;

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
    }
}