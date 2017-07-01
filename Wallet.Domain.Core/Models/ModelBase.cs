using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        [Required, Column(TypeName = "datetime")]
        public DateTime CreatedDate { get; set; }
        
        /// <summary>
        /// Gets or sets updated date
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedDate { get; set; }
    }
}