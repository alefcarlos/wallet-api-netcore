using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Wallet.Domain.Interfaces.Repositories
{
    using Domain.Core.Models;
    using Domain.Models;

    /// <summary>
    /// Interface contract for Repository
    /// </summary>
    public interface IRepositoryBase<T> where T : ModelBase
    {
        /// <summary>
        /// Gets all records
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<T>> GetAllAsync();
        /// <summary>
        /// Gets an specific record by id
        /// </summary>
        /// <param name="id">record id</param>
        /// <returns>Retuns the found record</returns>
        Task<T> GetAsync(int id);

        /// <summary>
        /// It adds a record
        /// </summary>
        /// <param name="entity">Model to add</param>
        /// <returns></returns>
        Task AddAsync(T entity);

        /// <summary>
        /// It removes a record
        /// </summary>
        /// <param name="entity">Model to remove</param>
        /// <returns></returns>
        Task DeleteAsync(T entity);

        /// <summary>
        /// It removes a record by id
        /// </summary>
        /// <param name="id">record id</param>
        Task DeleteAsync(int id);
        
        Task UpdateAsync(T entity);

        /// <summary>
        /// It allows to create queries
        /// </summary>
        /// <returns>Returns an IQueryable instance</returns>
        IQueryable<T> Query();

        /// <summary>
        /// This method is executed before AddAsyc()
        /// </summary>
        /// <returns>Returns an IQueryable instance</returns>

        void BeforeAdd(T entity);
        /// <summary>
        /// Verifies if an record exists by id
        /// </summary>
        /// <param name="id">record id</param>
        /// <returns>Returns true if the record existis, otherwise false</returns>
        bool Exists(int id);

        int ExecuteQuery(string sql, params object[] paramaters);
    }
}