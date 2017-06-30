using System.ComponentModel.DataAnnotations;

namespace Wallet.Api.Models.VM
{
    public class WalletUserVM
    {
        public int WalletUserId { get; set; }
        
        /// <summary>
        /// Nome do usuário
        /// </summary>
        [Required,
        MaxLength(20)]
        public string Name { get; set; }

        /// <summary>
        /// E-mail do usuário
        /// </summary>
        [Required,
        MaxLength(100)]
        public string Email { get; set; }
    }
}