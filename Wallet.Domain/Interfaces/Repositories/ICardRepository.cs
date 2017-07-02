
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
        /// Gets all cards by logged user
        /// </summary>
        /// <returns>Returns a card list.</returns>
        Task<List<Card>> GetByLoggedUser();

        /// <summary>
        /// Gets all user's cards with limit > 0
        /// </summary>
        /// <returns>Return a card list</returns>
        Task<List<Card>> GetAllAvailableLimitAsync();

        /// <summary>
        /// Gets the cards's total limit 
        /// </summary>
        /// <returns>Return the total limit for this user</returns>
        Task<decimal> GetSumLimit();

        /// <summary>
        /// Subtract a value from available card limit.
        /// </summary>
        /// <param name="cardId">Card id</param>
        /// <param name="value">Value to subtract</param>
        void SubtractLimit(int cardId, decimal value);

        /// <summary>
        /// Releases a value from available card limit.
        /// </summary>
        /// <param name="cardId">Card id</param>
        /// <param name="value">Value to release</param>
        void ReleaseLimit(int cardId, decimal value);
    }
}