using Demo.WalletManagement.Api.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Demo.WalletManagement.Api.data;

public class ApiDbContext : DbContext
{
    public ApiDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Wallet> Wallets { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var wallets = new List<Wallet>()
        {
            new Wallet
            {
                Balance = 500.00,
                Id = 1,
                AccountName = "First Account",
                AccountNumber = "001424244540"
            },
            new Wallet
            {
                Balance = 400.00,
                Id = 2,
                AccountName = "Second Account",
                AccountNumber = "5256344540"
            },
            new Wallet
            {
                Balance = 540.00,
                Id = 3,
                AccountName = "Third Account",
                AccountNumber = "425424244540"
            },
            new Wallet
            {
                Balance = 70.00,
                Id = 4,
                AccountName = "Fourth Account",
                AccountNumber = "7014251244540"
            },
            new Wallet
            {
                Balance = 10000.00,
                Id = 5,
                AccountName = "Fifth Account",
                AccountNumber = "101424244540"
            },
        };
        modelBuilder.Entity<Wallet>().HasData(wallets);
    }
}