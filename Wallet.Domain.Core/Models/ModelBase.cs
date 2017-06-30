using System;
using System.ComponentModel.DataAnnotations;

namespace Wallet.Domain.Core.Models
{
    /// <summary>
    /// Base class for entity Model
    /// </summary>
    public class ModelBase
    {
        /// <summary>
        /// Gets or sets created date
        /// It's required
        /// </summary>
        [Required]
        public DateTime CreatedDate { get; set; }
        
        /// <summary>
        /// Gets or sets updated date
        /// </summary>
        public DateTime? UpdatedDate { get; set; }
    }
}