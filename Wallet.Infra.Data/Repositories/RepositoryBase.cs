using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Wallet.Infra.Data.Repositories
{
    using Domain.Core.Models;
    using Domain.Interfaces.Repositories;
    using Wallet.Domain.Core.Exceptions;

    /// <summary>
    /// Repository base for repository pattern
    /// </summary>
    public abstract class RepositoryBase<T> : IRepositoryBase<T>
        where T : ModelBase
    {

        /// <summary>
        /// Shared context instance.
        /// It comes from DI
        /// </summary>
        protected readonly WalletContext dataContext;
        // protected readonly IUserManagment _userManagment;

        /// <summary>
        /// logger instance
        /// It comes from DI
        /// </summary>
        protected readonly ILogger _logger;

        public RepositoryBase(WalletContext context,
        //                          ,IUserManagment userManagment,
                                 ILogger logger)
        {
            dataContext = context;
            // _userManagment = userManagment;
            _logger = logger;
        }

        /// <summary>
        /// Get all records
        /// </summary>
        /// <returns>Returns a list of T</returns>
        public virtual async Task<IEnumerable<T>> GetAllAsync() => await Set().AsNoTracking().ToListAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">Record Id</param>
        /// <returns></returns>
        public virtual async Task<T> GetAsync(int id)
        {
            var entity = await Set().FindAsync(id);

            if (entity == null)
                throw new RecordNotFoundException();

            return entity;
        }

        public virtual async Task AddAsync(T entity)
        {
            BeforeAdd(entity);

            Set().Add(entity);

            await SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(T entity)
        {
            Set().Remove(entity);
            await SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(int id)
        {
            try
            {
                var entity = await GetAsync(id);

                if (entity == null)
                    throw new RecordNotFoundException();

                Set().Remove(entity);
                await SaveChangesAsync();
            }
            catch
            {
                throw;
            }
        }

        public virtual async Task UpdateAsync(T entity)
        {
            BeforeUpdate(entity);
            Set().Update(entity);
            await SaveChangesAsync();
        }

        public IQueryable<T> Query() => Set().AsQueryable();

        public virtual void BeforeAdd(T entity)
        {
            //Popular propriedades padrões
            entity.CreatedDate = DateTime.Now;
        }

        public virtual void BeforeUpdate(T entity)
        {
            entity.UpdatedDate = DateTime.Now;
        }

        public virtual bool Exists(int ids) => GetAsync(ids) != null;

        public int ExecuteQuery(string sql, params object[] paramaters)
        {
            // var result = dataContext.Database.ExecuteSqlCommand(sql, paramaters);
            // dataContext.SaveChanges();

            // return result;
            return -1;
        }

        private DbSet<T> Set() => dataContext.Set<T>();

        private Task SaveChangesAsync() => dataContext.SaveChangesAsync();
    }
}