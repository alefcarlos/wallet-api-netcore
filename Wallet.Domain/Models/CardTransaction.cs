using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Wallet.Domain.Models
{
    using System.ComponentModel.DataAnnotations.Schema;
    using Domain.Core.Models;

    public class CardTransaction : ModelBase
    {
        [Key]
        public int CardTransactionId { get; set; }

        public Card Card { get; set; }
        public int CardId { get; set; }

        [Required]
        public decimal Value { get; set; }

        [Required, MaxLength(50)]
        public string Description { get; set; }

        [Required, Column(TypeName = "datetime")]
        public DateTime Date { get; set; }

        public ECardTransactionType Type { get; set; } = ECardTransactionType.Purchase;
    }

    public enum ECardTransactionType
    {
        Purchase,

        ReleaseCredit,
    }
}