using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Wallet.Api.Models.VM
{
    public class WalletUserVM
    {
        public int WalletUserId { get; set; }

        [Required,
        MaxLength(20)]
        public string Name { get; set; }

        [Required,
        MaxLength(100)]
        public string Email { get; set; }


        public decimal RealLimit { get; set; } = 0;

        public List<CardVM> CardsInfo { get; set; } = new List<CardVM>();
    }
}