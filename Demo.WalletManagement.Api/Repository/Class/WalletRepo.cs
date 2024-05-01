using Demo.WalletManagement.Api.data;
using Demo.WalletManagement.Api.Models.Domain;
using Demo.WalletManagement.Api.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace Demo.WalletManagement.Api.Repository.Class;

public class WalletRepo : IWalletRepo
{
    private readonly ApiDbContext _context;

    public WalletRepo(ApiDbContext context)
    {
        _context = context;
    }

    public async Task<List<Wallet>> GetAllAsync()
    {
        var results = await _context.Wallets.ToListAsync();
        return results;
    }

    public async Task<Wallet?> GetByIdAsync(int id)
    {
        var existing = await _context.Wallets.FindAsync(id);

        return existing;
    }

    public async Task<Wallet?> CreateAsync(Wallet wallet)
    {
        var result = await _context.Wallets.FirstOrDefaultAsync(x => x.AccountNumber == wallet.AccountNumber);
        if (result is null)
        {
           var data =  await _context.Wallets.AddAsync(wallet);
            await _context.SaveChangesAsync();
            return data.Entity;
        }

        return null;
    }

    public async Task<Wallet?> UpdateAsync(int id, Wallet wallet)
    {
        var existingWallet = await _context.Wallets.FindAsync(id);
        if (existingWallet is null)
        {
            return null;
        }

        existingWallet.AccountNumber = wallet.AccountNumber;
        existingWallet.Balance = wallet.Balance;
        existingWallet.AccountName = wallet.AccountName;

        await _context.SaveChangesAsync();
        return existingWallet;
    }

    public async Task<Wallet?> RemoveAsync(int id)
    {
        var existingWallet = await _context.Wallets.FindAsync(id);
        if (existingWallet is null)
        {
            return null;
        }

        _context.Wallets.Remove(existingWallet);
        await _context.SaveChangesAsync();
        return existingWallet;
    }
}