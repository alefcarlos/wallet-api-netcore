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
    /// Card transactions controller
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/cards")]
    [ValidateApiUser]
    [ProducesResponseType(typeof(ApiResult<string>), 401)]
    [ProducesResponseType(typeof(ApiResult<string>), 400)]
    [ProducesResponseType(typeof(ApiResult<string>), 404)]
    [ProducesResponseType(typeof(ApiResult<string>), 200)]
    public class CardTransactionController : ControllerBase<CardTransaction>
    {
        public CardTransactionController(ICardTrasactionRepository repo,
                                    ILogger<CardTransaction> logger,
                                    IMapper mapper)
            : base(repo, logger, mapper)
        {
        }

        /// <summary>
        /// Gets all transactions from a card.
        /// </summary>
        /// <returns>Returns all users.</returns>
        [HttpGet("{cardId:int}/transactions")]
        [ProducesResponseType(typeof(ApiResult<IEnumerable<CardTransactionVM>>), 200)]
        public async Task<IActionResult> Get(int cardId)
        {
            try
            {
                var list = await (_repository as ICardTrasactionRepository).GetAllByCardIdAsync(cardId);

                return ReturnOk(_mapper.Map<IEnumerable<CardTransaction>, IEnumerable<CardTransactionVM>>(list));
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
        /// Adds a card transaction.
        /// To release limit: Inform a cardid and type:1
        /// </summary>
        /// <returns>Returns Ok/Error.</returns>
        [HttpPost("transaction")]
        [ValidateModel]
        [ProducesResponseType(typeof(ApiResult<string>), 200)]
        public async Task<IActionResult> Post([FromBody]NewCardTransactionInfoVM vm)
        {
            try
            {
                var entity = _mapper.Map<NewCardTransactionInfoVM, CardTransaction>(vm);

                await (_repository as ICardTrasactionRepository).AddNewTransactionAsync(entity);

                return ReturnOk("Incluído com sucesso");
            }
            catch (NotAllowedException)
            {
                return ReturnUnauthorized();
            }
            catch (Exception ex)
            {
                return ReturnError(ex.Message);
            }
        }
    }
}