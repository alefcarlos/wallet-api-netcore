using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using System.Net;
using System;
using Wallet.Api.Models;
using Wallet.Domain.Interfaces.Repositories;
using Wallet.Domain.Core.Exceptions;
using Wallet.Domain.Interfaces.User;

namespace Wallet.Api.Filters
{
    /// <summary>
    /// Validates if a user is logged-in
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class ValidateApiUserAttribute : TypeFilterAttribute
    {
        public ValidateApiUserAttribute() : base(typeof(ValidateApiUserImpl))
        {
        }

        private class ValidateApiUserImpl : IActionFilter
        {
            private readonly ApiSettings _apiOptions;
            private readonly ILogger _logger;
            private readonly IWalletUserRepository _userRepository;
            private IUserManagment _userManagment;
            public ValidateApiUserImpl(ILoggerFactory loggerFactory,
                                        IOptions<ApiSettings> apiOptions,
                                        IWalletUserRepository userRepository,
                                        IUserManagment userManagment)
            {
                _logger = loggerFactory.CreateLogger<ValidateApiUserImpl>();
                _apiOptions = apiOptions.Value;
                _userRepository = userRepository;
                _userManagment = userManagment;
            }

            public void OnActionExecuting(ActionExecutingContext context)
            {
                _logger.LogInformation("Validando informacoes de acesso a api...");
                _logger.LogInformation($"Nome do header => {_apiOptions.HeaderIdentityName}");
                _logger.LogInformation($"tem o header ? => { context.HttpContext.Request.Headers.ContainsKey(_apiOptions.HeaderIdentityName)}");

                if (!context.HttpContext.Request.Headers.ContainsKey(_apiOptions.HeaderIdentityName))
                {
                    context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    context.Result = new JsonResult(ApiResult<string>.GetUnauthorizedResult("Usuário não tem permissão para essa requisição."));
                }
                else
                {
                    var key = context.HttpContext.Request.Headers.First(x => x.Key == _apiOptions.HeaderIdentityName);

                    try
                    {
                        //validar se existe essa chave(guid) e obter o usuário
                        var user = _userRepository.ValidadeToken(key.Value);
                        _userManagment.SetUser(user);
                        _logger.LogInformation($"Usuário {user.Name} fez requisição.");
                    }
                    catch (RecordNotFoundException)
                    {
                        context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        context.Result = new JsonResult(ApiResult<string>.GetUnauthorizedResult("Usuário não tem permissão para essa requisição."));
                    }
                    catch (Exception ex)
                    {
                        context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        context.Result = new JsonResult(new ApiResult<string>
                        {
                            Code = HttpStatusCode.BadRequest,
                            Done = false,
                            Data = ex.Message
                        });
                    }

                }
            }

            public void OnActionExecuted(ActionExecutedContext context)
            {
            }
        }
    }
}