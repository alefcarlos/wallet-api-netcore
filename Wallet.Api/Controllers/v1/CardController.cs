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
    using Wallet.Domain.Interfaces.User;


    /// <summary>
    /// Card controller
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/cards")]
    [ValidateApiUser]
    [ProducesResponseType(typeof(ApiResult<string>), 401)]
    [ProducesResponseType(typeof(ApiResult<string>), 400)]
    [ProducesResponseType(typeof(ApiResult<string>), 404)]
    [ProducesResponseType(typeof(ApiResult<string>), 200)]
    public class CardController : ControllerBase<Card>
    {
        private readonly IUserManagment _userManagment;

        public CardController(ICardRepository repo,
                                    ILogger<CardController> logger,
                                    IMapper mapper,
                                    IUserManagment userManagment)
            : base(repo, logger, mapper)
        {
            _userManagment = userManagment;
        }

        /// <summary>
        /// Gets all cards in db.
        /// It requires admin permission.
        /// </summary>
        /// <returns>Returns all users.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResult<IEnumerable<CardVM>>), 200)]
        [ValidateApiUserRole(EUserManagmentRole.Admin)]
        public async Task<IActionResult> Get()
        {
            var list = await _repository.GetAllAsync();

            return ReturnOk(_mapper.Map<IEnumerable<Card>, IEnumerable<CardVM>>(list));
        }

        /// <summary>
        /// Gets an specifc card by id.
        /// </summary>
        /// <returns>Returns an specific card</returns>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var entity = await _repository.GetAsync(id);

                if (entity.WalletUserId != _userManagment.User.WalletUserId)
                    throw new NotAllowedException();

                return ReturnOk(_mapper.Map<Card, CardVM>(entity));
            }
            catch (NotAllowedException)
            {
                return ReturnUnauthorized();
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
        /// Adds a card for an user.
        /// </summary>
        /// <returns>Returns Ok/Error.</returns>
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Post([FromBody]Card entity)
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

        /// <summary>
        /// Get all logged user's cards
        /// </summary>
        /// <returns>Returns Ok/Error.</returns>
        [HttpGet("user")]
        public async Task<IActionResult> GetByUserId()
        {
            try
            {
                var list = await (_repository as ICardRepository).GetByLoggedUser();

                return ReturnOk(_mapper.Map<IEnumerable<Card>, IEnumerable<CardVM>>(list));
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
        /// Delete a card.
        /// </summary>
        /// <returns>Returns Ok/Error.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _repository.DeleteAsync(id);

                return ReturnOk("Deletado com sucesso");
            }
            catch (NotAllowedException)
            {
                return ReturnUnauthorized();
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
    }
}