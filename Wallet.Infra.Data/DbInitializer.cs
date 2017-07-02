using System;
using System.Linq;
using Wallet.Domain.Models;

namespace Wallet.Infra.Data
{
    public static class DbInitializer
    {
        public static void Initialize(WalletContext context)
        {
            //Creating the db
            context.Database.EnsureCreated();

            // DB has been seeded
            if (context.Users.Any())
                return;

            //Adding the admin user
            context.Users.Add(new WalletUser
            {
                Name = "admin",
                Email = "admin@system.com",
                Role = EUserManagmentRole.Admin,
                Code = Guid.NewGuid(),
                CreatedDate = DateTime.Now
            });

            context.SaveChanges();
        }
    }
}