using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Wallet.Infra.Data
{
    using Wallet.Domain.Models;

    /// <summary>
    /// Context class for EntityFramewirk
    /// </summary>
    public class WalletContext : DbContext
    {
        /// <summary>
        /// Users Set
        /// </summary>
        public DbSet<WalletUser> Users { get; set; }

        /// <summary>
        /// Credit cards set
        /// </summary>
        public DbSet<Card> Cards { get; set; }

        public WalletContext(DbContextOptions<WalletContext> options) :
            base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Card Number must be unique
            modelBuilder.Entity<Card>()
                .HasIndex(c=> c.Number)
                .IsUnique();
        }
    }
}
