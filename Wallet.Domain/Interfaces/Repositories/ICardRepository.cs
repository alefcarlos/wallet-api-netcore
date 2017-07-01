
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
        Task<List<Card>> GetAllAvailableLimitAsync();

        /// <summary>
        /// Gets the cards's total limit 
        /// </summary>
        /// <returns>Return the total limit for this user</returns>
        Task<decimal> GetSumLimit();

        void SubtractLimit(int cardId, decimal value);
    }
}