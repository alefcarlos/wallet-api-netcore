using System.Net;

namespace Wallet.Api.Models
{
    /// <summary>
    /// Default api result Model
    /// </summary>
    public class ApiResult<T>
    {
        /// <summary>
        /// Result code
        /// </summary>
        public HttpStatusCode Code { get; set; }

        /// <summary>
        /// Specifies if the result is ok
        /// </summary>
        public bool Done { get; set; }

        /// <summary>
        /// return Message
        /// </summary>
        /// <returns></returns>
        public T Data { get; set; }

        /// <summary>
        /// A generic method to return 401
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ApiResult<T> GetUnauthorizedResult(T data)
        {
            return new ApiResult<T>
            {
                Code = HttpStatusCode.Unauthorized,
                Done = false,
                Data = data
            };
        }
    }
}