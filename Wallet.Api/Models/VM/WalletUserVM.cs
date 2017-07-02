using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

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

        public decimal RealLimit { get; set; }

        public decimal AvailableLimit  => CardsInfo.Sum(x => x.AvailableLimit);

        public decimal CardsLimit => CardsInfo.Sum(x => x.Limit);

        public List<CardVM> CardsInfo { get; set; } = new List<CardVM>();
    }
}