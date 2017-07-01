using Wallet.Domain.Interfaces.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace Wallet.IoC
{
    using Infra.Data;
    using Infra.Data.Repositories;
    using Wallet.Domain;
    using Wallet.Domain.Interfaces.User;

    /// <summary>
    /// Class to use in Startup.cs
    /// </summary>
    public static class ServicesExtensions
    {
        /// <summary>
        /// It injects the repositories in container.
        /// </summary>
        /// <param name="services"></param>
        public static void AddRepositories(this IServiceCollection services)
        {
            //Scoped lifetime services are created once per request.
            services.AddScoped<IWalletUserRepository, WalletUserRepository>();
            services.AddScoped<ICardRepository, CardRepository>();
            services.AddScoped<ICardTrasactionRepository, CardTransactionRepository>();
        }

        public static void AddUser(this IServiceCollection services)
        {
            services.AddScoped<IUserManagment, UserManagment>();
        }

        /// <summary>
        /// It injects the WalletContext in container.
        /// </summary>
        /// <param name="services"></param>
        public static void AddEF(this IServiceCollection services)
        {
            services.AddEntityFrameworkInMemoryDatabase()
                .AddDbContext<WalletContext>(options =>
                {
                    //Just for tests
                    options.UseInMemoryDatabase();
                });
        }
    }
}