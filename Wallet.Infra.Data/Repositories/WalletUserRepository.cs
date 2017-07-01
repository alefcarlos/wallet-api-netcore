using System;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Wallet.Infra.Data.Repositories
{
    using Domain.Models;
    using Domain.Interfaces.Repositories;
    using Domain.Core.Exceptions;
    using System.Linq;
    using Wallet.Domain.Interfaces.User;

    public class WalletUserRepository : RepositoryBase<WalletUser>, IWalletUserRepository
    {
        //Allow us to handle the logged user 
        private readonly IUserManagment _userManagment;

        private readonly ICardRepository _cardRepository;
        public WalletUserRepository(WalletContext context,
                                    ILogger<WalletUserRepository> logger,
                                    IUserManagment userManagment,
                                    ICardRepository cardRepository)
            : base(context, logger)
        {
            _userManagment = userManagment;
            _cardRepository = cardRepository;
        }

        public override void BeforeAdd(WalletUser entity)
        {
            entity.Code = Guid.NewGuid();

            base.BeforeAdd(entity);
        }

        /// <summary>
        /// Gets the wallet informar from an user 
        /// </summary>
        /// <returns>Returns cards and transactions from wallet</returns>
        public async Task<WalletUser> GetInfoAsync()
        {
            var entity = await Query()
                            .Include(card => card.Cards)
                                .ThenInclude(t => t.Transactions)
                            .FirstOrDefaultAsync(x => x.WalletUserId == _userManagment.User.WalletUserId);

            if (entity == null)
                throw new RecordNotFoundException();

            return entity;
        }

        /// <summary>
        /// It logins an user.
        /// </summary>
        /// <param name="email">User email</param>
        /// <param name="password">User passowrd</param>
        /// <returns>Returns the user entity</returns>
        public async Task<WalletUser> Login(string email, string password)
        {
            var entity = await Query().FirstOrDefaultAsync(x => x.Email == email);

            if (entity == null)
                throw new RecordNotFoundException("E-mail ou senha incorretos!");

            //Validating password
            //;...

            return entity;
        }

        /// <summary>
        /// Updates the user's real limit
        /// </summary>
        public async Task UpdatedRealLimit(decimal value)
        {
            var limit = await _cardRepository.GetSumLimit();

            if (value > limit)
                throw new Exception($"The real limit must be less or equals than {limit.ToString("C")}");

            var entity = await GetAsync(_userManagment.User.WalletUserId);
            entity.RealLimit = value;

            await UpdateAsync(entity);
        }

        /// <summary>
        /// Validates if a token is valid
        /// </summary>
        /// <param name="code">Guid to validade</param>
        /// <returns>Returns the related user.</returns>
        public WalletUser ValidadeToken(string code)
        {
            Guid _guid;

            //Converter em Guid
            var success = Guid.TryParse(code, out _guid);

            if (!success)
                throw new RecordNotFoundException();

            var model = Query().AsNoTracking().FirstOrDefault(x => x.Code == _guid);

            if (model == null)
                throw new RecordNotFoundException();

            return model;
        }
    }
}