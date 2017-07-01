using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Wallet.Api.Models;
using Wallet.Domain.Models;
using Wallet.Domain.Interfaces.User;
using System.Net;

namespace Wallet.Api.Filters
{
    /// <summary>
    /// Atributo para validação de permissão do usuário da API
    /// </summary>
    public class ValidateApiUserRoleAttribute : TypeFilterAttribute
    {
        public ValidateApiUserRoleAttribute(EUserManagmentRole AllowRole) : base(typeof(ValidateApiUserRoleAttributeImpl))
        {
            Arguments = new object[] { AllowRole };
        }

        private class ValidateApiUserRoleAttributeImpl : IActionFilter
        {
            private readonly ILogger _logger;
            private IUserManagment _userManagment;
            private EUserManagmentRole _allowRole;
            public ValidateApiUserRoleAttributeImpl(ILoggerFactory loggerFactory,
                                                    IUserManagment userManagment,
                                                    EUserManagmentRole AllowRole)
            {
                _logger = loggerFactory.CreateLogger<ValidateApiUserRoleAttributeImpl>();
                _userManagment = userManagment;
                _allowRole = AllowRole;
            }

            public void OnActionExecuting(ActionExecutingContext context)
            {
                //Validar se o usuário tem permissão
                if (!_userManagment.CanExecute(_allowRole)){
                    context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    context.Result = new JsonResult(ApiResult<string>.GetUnauthorizedResult("Usuário não tem permissão para essa requisição."));
                }
            }

            public void OnActionExecuted(ActionExecutedContext context)
            {
            }
        }
    }
}