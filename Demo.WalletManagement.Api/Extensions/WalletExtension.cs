using Demo.WalletManagement.Api.data;
using Demo.WalletManagement.Api.Repository.Class;
using Demo.WalletManagement.Api.Repository.Interface;
using Demo.WalletManagement.Api.Services.Cache;
using Microsoft.EntityFrameworkCore;

namespace Demo.WalletManagement.Api.Extensions;

public static class WalletExtension
{
    public static IServiceCollection AddWalletServices(this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        serviceCollection.AddControllers();
        serviceCollection.AddEndpointsApiExplorer();
        serviceCollection.AddSwaggerGen();
        serviceCollection.AddDbContext<ApiDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
        
        serviceCollection.AddScoped<IWalletRepo, WalletRepo>();
        serviceCollection.AddScoped<ICacheService, CacheService>();
        return serviceCollection;
    }
}