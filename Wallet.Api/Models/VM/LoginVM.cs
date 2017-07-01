using System.ComponentModel.DataAnnotations;

namespace Wallet.Api.Models.VM
{
    public class LoginVM
    {
        [Required, DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}