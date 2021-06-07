using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CachingDemo.Infrastructure;
using DistributedCache.Models;
using Microsoft.Extensions.Caching.Distributed;

namespace CachingDemo.Services
{
    public class CachedUserService : IUserService
    {
        private const int CacheTTL = 120;
        private readonly UserService _userService;
        private readonly ICacheProvider _cacheProvider;

        private static readonly SemaphoreSlim GetUsersSemaphore = new(1, 1);

        public CachedUserService(UserService userService, ICacheProvider cacheProvider)
        {
            _userService = userService;
            _cacheProvider = cacheProvider;
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await GetCachedResponse(CacheKeys.Users, GetUsersSemaphore, () => _userService.GetUsersAsync());
        }

        private async Task<IEnumerable<User>> GetCachedResponse(string cacheKey, SemaphoreSlim semaphore, Func<Task<IEnumerable<User>>> func){
            var users = await _cacheProvider.GetFromCache<IEnumerable<User>>(cacheKey);

            if(users != null) return users;
            try{
                await semaphore.WaitAsync();
                users = await _cacheProvider.GetFromCache<IEnumerable<User>>(cacheKey);
                
                if (users != null) return users;

                users = await func();
                var cacheEntryOptions = new DistributedCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromSeconds(CacheTTL));

                await _cacheProvider.SetCache(cacheKey, users, cacheEntryOptions);
            } finally{
                semaphore.Release();
            }

            return users;
        }
    }
}