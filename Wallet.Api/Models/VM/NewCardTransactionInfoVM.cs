using System;
using System.ComponentModel.DataAnnotations;
using Wallet.Domain.Models;

namespace Wallet.Api.Models.VM
{
    public class NewCardTransactionInfoVM
    {
        [Required]
        public decimal Value { get; set; }

        [Required]
        public string Description { get; set; }

        public ECardTransactionType Type { get; set; } = ECardTransactionType.Purchase;
    }
}