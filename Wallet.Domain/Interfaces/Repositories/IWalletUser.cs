
namespace Wallet.Domain.Interfaces.Repositories
{
    using Domain.Core.Models;
    using Domain.Models;

    /// <summary>
    /// Waller user interface for repository pattern
    /// </summary>
    public interface IWalletUserRepository : IRepositoryBase<WalletUser>
    {
    }
}