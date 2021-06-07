using System.Collections.Generic;
using System.Threading.Tasks;
using CachingDemo.Infrastructure;
using DistributedCache.Models;

namespace CachingDemo.Services{
    public interface IUserService{
        Task<IEnumerable<User>> GetUsersAsync();
    }

    public class UserService : IUserService{
        private readonly IHttpClient _httpClient;

        public UserService(IHttpClient httpClient){
            _httpClient = httpClient;
        }

        public Task<IEnumerable<User>> GetUsersAsync()
        {
            return _httpClient.Get();
        }
    }
}