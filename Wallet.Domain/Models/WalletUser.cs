using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace Wallet.Domain.Models
{

    using Domain.Core.Models;

    /// <summary>
    /// Its both the user and Wallet
    /// </summary>
    [Table("WalletUser")]
    public class WalletUser : ModelBase
    {
        [Key]
        public int WalletUserId { get; set; }

        /// <summary>
        /// Gets or sets user name
        /// </summary>
        [Required,
        MaxLength(20)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets user email
        /// </summary>
        [Required,
        MaxLength(100)]
        public string Email { get; set; }

        /// <summary>
        /// Get or sets User code. It is generated on creation
        /// </summary>
        [Required]
        public Guid Code { get; set; } = Guid.Empty;

        /// <summary>
        /// Gets or sets the real limit, it was set by user.
        /// It can not surpass maximum limit.
        /// </summary>
        public decimal RealLimit { get; set; }

        /// <summary>
        /// Gets or sets User role
        /// </summary>
        [Required]
        public EUserManagmentRole Role { get; set; }

        public IEnumerable<Card> Cards { get; set; }
    }

    /// <summary>
    /// User Roles
    /// </summary>
    public enum EUserManagmentRole
    {
        /// <summary>
        /// It is a basic User
        /// </summary>
        Basic = 0,

        /// <summary>
        /// It is a admin user.
        /// They can list users...
        /// </summary>
        Admin = 1,
    }
}