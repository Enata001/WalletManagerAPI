using System.Text.Json;
using StackExchange.Redis;

namespace Demo.WalletManagement.Api.Services.Cache;

public class CacheService: ICacheService
{

    private readonly IDatabase _cache;

    public CacheService( IConfiguration configuration)
    {
        var redis = ConnectionMultiplexer.Connect(configuration["host"]!);
        _cache = redis.GetDatabase();
    }
    public T GetData<T>(string key)
    {
        var value = _cache.StringGet(key);
        if (string.IsNullOrEmpty(value))
        {
            return default!;
        }

        return JsonSerializer.Deserialize<T>(value!)!;
    }

    public bool SetData<T>(string key, T value, DateTimeOffset expirationTime)
    {
        var expiryTime = expirationTime.DateTime.Subtract(DateTime.Now);
        var isSet = _cache.StringSet(key, JsonSerializer.Serialize(value), expiryTime);
        return isSet;
    }

    public object RemoveData(string key)
    {
        var exist = _cache.KeyExists(key);
        if (!exist)
        {
            return false;
        }
        return _cache.KeyDelete(key);
    }
}