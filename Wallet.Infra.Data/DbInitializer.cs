using System;
using System.Linq;

namespace Wallet.Infra.Data
{
    public static class DbInitializer
    {
        public static void Initialize(WalletContext context)
        {
            context.Database.EnsureCreated();
        }
    }
}