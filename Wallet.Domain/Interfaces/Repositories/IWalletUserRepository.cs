
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
        /// <param name="userId">UserId requested</param>
        /// <returns>Returns cards and transactions from wallet</returns>
        Task<WalletUser> GetInfoAsync(int userId);
    }
}