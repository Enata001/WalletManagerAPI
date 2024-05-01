using Demo.WalletManagement.Api.Models.Domain;

namespace Demo.WalletManagement.Api.Repository.Interface;

public interface IWalletRepo
{
    Task<List<Wallet>> GetAllAsync();
    Task<Wallet?> GetByIdAsync(int id);
    Task<Wallet?> CreateAsync(Wallet wallet);
    Task<Wallet?> UpdateAsync(int id, Wallet wallet);
    Task<Wallet?> RemoveAsync(int id);
}