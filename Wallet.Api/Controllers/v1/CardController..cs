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
    /// User controller
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/cards")]
    public class CardController : ControllerBase<Card>
    {
        public CardController(ICardRepository repo,
                                    ILogger<CardController> logger,
                                    IMapper mapper)
            : base(repo, logger, mapper)
        {
        }

        /// <summary>
        /// Get all Users.
        /// It requires admin permission.
        /// </summary>
        /// <returns>Returns all users.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResult<IEnumerable<CardVM>>), 200)]
        [ProducesResponseType(typeof(ApiResult<string>), 400)]
        public async Task<IActionResult> Get()
        {
            var list = await _repository.GetAllAsync();

            return ReturnOk(_mapper.Map<IEnumerable<Card>, IEnumerable<CardVM>>(list));
        }

        /// <summary>
        /// Get an specifc user by id.
        /// It requires admin permission.
        /// </summary>
        /// <returns>Returns an specific user.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResult<CardVM>), 200)]
        [ProducesResponseType(typeof(ApiResult<string>), 400)]
        [ProducesResponseType(typeof(ApiResult<string>), 404)]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var entity = await _repository.GetAsync(id);

                return ReturnOk(_mapper.Map<Card, CardVM>(entity));
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
        /// Add an user.
        /// It requires admin permission.
        /// </summary>
        /// <returns>Returns Ok/Error.</returns>
        [HttpPost]
        [ValidateModel]
        [ProducesResponseType(typeof(ApiResult<string>), 200)]
        [ProducesResponseType(typeof(ApiResult<string>), 400)]
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

        [HttpGet("user/{id:int}")]
        [ProducesResponseType(typeof(ApiResult<CardVM>), 200)]
        [ProducesResponseType(typeof(ApiResult<string>), 400)]
        [ProducesResponseType(typeof(ApiResult<string>), 404)]
        public async Task<IActionResult> GetByUserId(int id)
        {
            try
            {
                var list = await (_repository as ICardRepository).GetByUserId(id);

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

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _repository.DeleteAsync(id);

                return ReturnOk("Deletado com sucesso");
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