
namespace Wallet.Domain.Interfaces.Repositories
{
    using System.Threading.Tasks;
    using Domain.Core.Models;
    using Domain.Models;

    /// <summary>
    /// Waller user interface for repository pattern
    /// </summary>
    public interface IWalletUserRepository : IRepositoryBase<WalletUser>
    {
        /// <summary>
        /// Gets the wallet informar from an user 
        /// </summary>
        /// <returns>Returns cards and transactions from wallet</returns>
        Task<WalletUser> GetInfoAsync();

        /// <summary>
        /// It logins an user.
        /// </summary>
        /// <param name="email">User email</param>
        /// <param name="password">User passowrd</param>
        /// <returns>Returns the user entity</returns>
        Task<WalletUser> Login(string email, string password);
        WalletUser ValidadeToken(string value);
        Task UpdatedRealLimit(decimal value);
    }
}