using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingService.Service.Interface
{
    public interface IRedisRepository
    {
        Task<T> GetAsync<T>(string key);
        Task<T> SetAsync<T>(string key, T value, int timeout = 60);
        Task<T> SetNotExpiredAsync<T>(string key, T value);
        Task RemoveAsync(string key);
        Task<bool> isExsit(string key);
    }
}
