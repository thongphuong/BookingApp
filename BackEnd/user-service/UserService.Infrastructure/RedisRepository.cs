using UserService.Service.Interface;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace UserService.Infrastructure
{
    public class RedisRepository : IRedisRepository
    {
        private readonly IDatabase _cache;

        public RedisRepository(IDatabase cache)
        {
            _cache = cache;
        }
        public async Task<T> GetAsync<T>(string key)
        {
            string value = await _cache.StringGetAsync(key);

            if (value != null)
            {
                return JsonSerializer.Deserialize<T>(value);
            }

            return default;
        }
        public async Task RemoveAsync(string key)
        {
            await _cache.KeyDeleteAsync(key);
        }
        public async Task<T> SetAsync<T>(string key, T value, int timeout = 60)
        {
            await _cache.StringSetAsync(key, JsonSerializer.Serialize(value), TimeSpan.FromMinutes(timeout));
            return value;
        }
        public async Task<T> SetNotExpiredAsync<T>(string key, T value)
        {
            await _cache.StringSetAsync(key, JsonSerializer.Serialize(value));
            return value;
        }
        public async Task<bool> isExsit(string key)
        {
            return !string.IsNullOrEmpty(await _cache.StringGetAsync(key));
        }
    }
}
