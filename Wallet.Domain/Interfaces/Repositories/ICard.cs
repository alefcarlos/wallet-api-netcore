
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Wallet.Domain.Interfaces.Repositories
{

    using Domain.Core.Models;
    using Domain.Models;

    /// <summary>
    /// Waller user interface for repository pattern
    /// </summary>
    public interface ICardRepository : IRepositoryBase<Card>
    {
        /// <summary>
        /// Gets all cards by user
        /// </summary>
        /// <param name="id">WallerUserId</param>
        /// <returns>Returns a card list.</returns>
        Task<IEnumerable<Card>> GetByUserId(int id);
    }
}