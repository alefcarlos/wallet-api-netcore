using Microsoft.AspNetCore.Mvc;
using System.Net;
using Microsoft.Extensions.Logging;

namespace Wallet.Api.Controllers
{
    using Domain.Core.Models;
    using Domain.Interfaces.Repositories;
    using Api.Models;
    using AutoMapper;

    public class ControllerBase<T> : Controller
        where T : ModelBase
    {

        protected readonly IRepositoryBase<T> _repository;
        protected readonly ILogger _logger;
        protected readonly IMapper _mapper;


        public ControllerBase(IRepositoryBase<T> repository,
                                ILogger logger,
                                IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Método para abstrair retorno OK 200
        /// </summary>
        /// <param name="data">Dados para retorna.</param>
        /// <returns>Retorna sucesso 200</returns>
        [NonAction]
        protected IActionResult ReturnOk<TData>(TData data){
            var result = new ApiResult<TData>{
                     Done = true,
                     Data = data,
                     Code = HttpStatusCode.OK
            };

            return Ok(result);
        }

        /// <summary>
        /// Método para abstrair retorno erro 400
        /// </summary>
        /// <param name="data">Dados para retorna.</param>
        /// <returns>Retorna erro 400</returns>
        [NonAction]
        protected IActionResult ReturnError<TData>(TData data){
            var result = new ApiResult<TData>{
                     Done = false,
                     Data = data,
                     Code = HttpStatusCode.BadRequest
            };

            return BadRequest(result);
        }

        /// <summary>
        /// Método para abstrair retorno error 404
        /// </summary>
        /// <param name="data">Dados para retorna.</param>
        /// <returns>Retorna erro 404</returns>
        [NonAction]
        protected IActionResult ReturnNotFound<TData>(TData data){
            var result = new ApiResult<TData>{
                     Done = false,
                     Data = data,
                     Code = HttpStatusCode.NotFound
            };

            return NotFound(result);
        }

        /// <summary>
        /// Método para abstrair retorno não autorizado 401
        /// </summary>
        /// <returns>Retorna erro 401</returns>
        [NonAction]
        protected IActionResult ReturnUnauthorized(){
            return new StatusCodeResult((int)HttpStatusCode.Unauthorized);
        }
    }
}