using System;
using System.ComponentModel.DataAnnotations;

namespace Wallet.Domain.Models
{
    using Domain.Core.Models;

    public class CardTransaction : ModelBase
    {
        [Key]
        public int CardTransactionId { get; set; }

        public Card Card { get; set; }
        public int CardId { get; set; }

        public decimal Value { get; set; }

        public string Description { get; set; }

        public DateTime Date { get; set; }

        public ECardTransactionType Type { get; set; }
    }

    public enum ECardTransactionType
    {
        Purchase,

        ReleaseCredit,
    }
}