using System.Threading.Tasks;
using System.Collections.Generic;

namespace Wallet.Domain.Interfaces.Repositories
{
    using Domain.Core.Models;
    using Domain.Models;

    /// <summary>
    /// Card transaction interface for repository pattern
    /// </summary>
    public interface ICardTrasactionRepository : IRepositoryBase<CardTransaction>
    {
        /// <summary>
        /// Gets all transactions by card
        /// </summary>
        /// <returns>Returns a transaction list.</returns>
        Task<IEnumerable<CardTransaction>> GetAllByCardIdAsync(int cardId);

        /// <summary>
        /// Adds a new card transaction
        /// </summary>
        /// <param name="entity">Transactions info</param>
        /// <returns></returns>
        Task AddNewTransactionAsync(CardTransaction entity);
    }
}