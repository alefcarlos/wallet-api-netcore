using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Wallet.Infra.Data;
using Wallet.Domain.Models;

namespace Wallet.Infra.Data.Migrations
{
    [DbContext(typeof(WalletContext))]
    [Migration("20170701184639_initial")]
    partial class initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Wallet.Domain.Models.Card", b =>
                {
                    b.Property<int>("CardId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CCV")
                        .IsRequired()
                        .HasMaxLength(4);

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime");

                    b.Property<int>("DueDate");

                    b.Property<DateTime>("ExpirationDate");

                    b.Property<decimal>("Limit");

                    b.Property<string>("Number")
                        .IsRequired()
                        .HasMaxLength(16);

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("datetime");

                    b.Property<int>("WalletUserId");

                    b.HasKey("CardId");

                    b.HasIndex("Number")
                        .IsUnique();

                    b.HasIndex("WalletUserId");

                    b.ToTable("Cards");
                });

            modelBuilder.Entity("Wallet.Domain.Models.CardTransaction", b =>
                {
                    b.Property<int>("CardTransactionId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CardId");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<int>("Type");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("datetime");

                    b.Property<decimal>("Value");

                    b.HasKey("CardTransactionId");

                    b.HasIndex("CardId");

                    b.ToTable("CardTransactions");
                });

            modelBuilder.Entity("Wallet.Domain.Models.WalletUser", b =>
                {
                    b.Property<int>("WalletUserId")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("Code");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.Property<decimal>("RealLimit");

                    b.Property<int>("Role");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("datetime");

                    b.HasKey("WalletUserId");

                    b.ToTable("WalletUser");
                });

            modelBuilder.Entity("Wallet.Domain.Models.Card", b =>
                {
                    b.HasOne("Wallet.Domain.Models.WalletUser", "Owner")
                        .WithMany("Cards")
                        .HasForeignKey("WalletUserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Wallet.Domain.Models.CardTransaction", b =>
                {
                    b.HasOne("Wallet.Domain.Models.Card", "Card")
                        .WithMany("Transactions")
                        .HasForeignKey("CardId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
