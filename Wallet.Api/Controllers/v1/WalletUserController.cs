using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AutoMapper;

namespace Wallet.Api.Controllers.v1
{
    using Domain.Interfaces.Repositories;
    using Domain.Models;
    using Api.Filters;
    using Domain.Core.Exceptions;
    using Wallet.Api.Models;
    using Wallet.Api.Models.VM;


    /// <summary>
    /// User/Wallet controller
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/users")]
    [ProducesResponseType(typeof(ApiResult<string>), 401)]
    [ProducesResponseType(typeof(ApiResult<string>), 400)]
    [ProducesResponseType(typeof(ApiResult<string>), 404)]
    [ProducesResponseType(typeof(ApiResult<string>), 200)]
    public class WalletUserController : ControllerBase<WalletUser>
    {
        public WalletUserController(IWalletUserRepository repo,
                                    ILogger<WalletUserController> logger,
                                    IMapper mapper)
            : base(repo, logger, mapper)
        {
        }

        /// <summary>
        /// Gets all Users.
        /// It requires admin permission.
        /// </summary>
        /// <returns>Returns all users.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResult<IEnumerable<WalletUserVM>>), 200)]
        [ValidateApiUser]
        [ValidateApiUserRole(EUserManagmentRole.Admin)]
        public async Task<IActionResult> Get()
        {
            _logger.LogInformation("teste");
            _logger.LogDebug("teste");
            _logger.LogError("teste");

            var list = await _repository.GetAllAsync();

            return ReturnOk(_mapper.Map<IEnumerable<WalletUser>, IEnumerable<WalletUserVM>>(list));
        }

        /// <summary>
        /// Gets logged user's complete infos
        /// </summary>
        /// <returns>Returns an specific user.</returns>
        [HttpGet("info")]
        [ProducesResponseType(typeof(ApiResult<WalletUserVM>), 200)]
        [ValidateApiUser]
        public async Task<IActionResult> GetInfo()
        {
            try
            {
                var entity = await (_repository as IWalletUserRepository).GetInfoAsync();

                var entityVM = _mapper.Map<WalletUser, WalletUserVM>(entity);

                return ReturnOk(entityVM);
            }
            catch (RecordNotFoundException ex)
            {
                return ReturnNotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return ReturnError(ex.Message);
            }
        }

        /// <summary>
        /// Adds an user.
        /// </summary>
        /// <returns>Returns Ok/Error.</returns>
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Post([FromBody]WalletUser entity)
        {
            try
            {
                await _repository.AddAsync(entity);

                return ReturnOk("Incluído com sucesso");
            }
            catch (Exception ex)
            {
                return ReturnError(ex.Message);
            }

        }

        // // PUT api/values/5
        // // [HttpPut("{id}")]
        // // public virtual async Task<IActionResult> Put(int id, [FromBody]T entity)
        // // {
        // //     try
        // //     {
        // //         await _repository.UpdateAsync(entity);

        // //         return ReturnOk("Alterado com sucesso");
        // //     }
        // //     catch (Exception ex)
        // //     {
        // //         return ReturnError(ex.Message);
        // //     }
        // // }

        // DELETE api/values/5
        // [HttpDelete("{id}")]
        // public async Task<IActionResult> Delete(int id)
        // {
        //     try
        //     {
        //         await _repository.DeleteAsync(id);

        //         return ReturnOk("Deletado com sucesso");
        //     }
        //     catch (RecordNotFoundException ex)
        //     {
        //         return ReturnNotFound(ex.Message);
        //     }
        //     catch (Exception ex)
        //     {
        //         return ReturnError(ex.Message);
        //     }
        // }

        /// <summary>
        /// User login
        /// </summary>
        /// <returns>Returns the user token.</returns>
        [HttpPost("login")]
        [ValidateModel]
        [ProducesResponseType(typeof(ApiResult<Guid>), 200)]
        public async Task<IActionResult> Login([FromBody]LoginVM login)
        {
            try
            {
                var user = await (_repository as IWalletUserRepository).Login(login.Email, login.Password);

                return ReturnOk(user.Code);
            }
            catch (RecordNotFoundException ex)
            {
                return ReturnNotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return ReturnError(ex.Message);
            }
        }

        /// <summary>
        /// Updates the logged user's real limit
        /// </summary>
        /// <returns>Returns</returns>
        [HttpPut("reallimit")]
        [ValidateModel]
        [ValidateApiUser]
        public async Task<IActionResult> UpdatedRealLimit([FromBody]decimal value)
        {
            try
            {
                await (_repository as IWalletUserRepository).UpdatedRealLimit(value);

                return ReturnOk("Alterado com sucesso.");
            }
            catch (Exception ex)
            {
                return ReturnError(ex.Message);
            }
        }
    }
}