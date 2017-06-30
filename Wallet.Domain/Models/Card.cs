using System;
using System.ComponentModel.DataAnnotations;


namespace Wallet.Domain.Models
{
    using Domain.Core.Models;

    /// <summary>
    /// Model for wallet card entity
    /// </summary>
    public class Card : ModelBase
    {
        [Key]
        public int CardId { get; set; }

        public WalletUser Owner { get; set; }


        /// <summary>
        /// Card's owner
        /// </summary>
        public int WalletUserId { get; set; }

        /// <summary>
        /// Gets or sets the card number
        /// </summary>
        [Required, MinLength(16), MaxLength(16)]
        public string Number { get; set; }

        /// <summary>
        /// Gets or sets the security code
        /// </summary>
        [Required, MaxLength(4)]
        public string CCV { get; set; }

        /// <summary>
        /// Gets or sets the expiration date
        /// </summary>
        [Required]
        public DateTime ExpirationDate { get; set; }

        /// <summary>
        /// Gets or sets the card due date
        /// </summary>
        [Required]
        public int DueDate { get; set; }

        /// <summary>
        /// Gets or sets the card amount limit 
        /// </summary>
        [Required]
        public decimal Limit { get; set; }
    }
}