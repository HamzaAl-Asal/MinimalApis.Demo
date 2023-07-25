using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using MinimalApis.Demo.Context;
using MinimalApis.Demo.Context.Models.Users;
using MinimalApis.Demo.Interfaces;
using MinimalApis.Demo.Models.Users;
using MinimalApis.Demo.Options;
using MinimalApis.Demo.Options.Extentions;

namespace MinimalApis.Demo.Services
{
    public class UserService : IUserService
    {
        private readonly IMemoryCache cache;
        private readonly ILogger<UserService> logger;
        private readonly IConfiguration configuration;
        private readonly IMapper mapper;
        private readonly MinimalApisContext context;

        public UserService(IMemoryCache cache,
            ILogger<UserService> logger,
            IConfiguration configuration,
            IMapper mapper,
            MinimalApisContext context)
        {
            this.cache = cache;
            this.logger = logger;
            this.configuration = configuration;
            this.mapper = mapper;
            this.context = context;
        }

        public async Task<Ok<IEnumerable<UserViewModel>>> GetUsersAsync()
        {
            if (cache.TryGetValue(Constant.GetUsersCacheKey, out IEnumerable<UserViewModel> users))
            {
                logger.LogInformation("Found Users List in cache");
            }
            else
            {
                logger.LogInformation("No Users List in cache..");
                logger.LogInformation("Fetching users from the database..");

                users = mapper.Map<IEnumerable<UserViewModel>>(context.User.ToList());

                if (users.Any())
                {
                    var options = OptionBinding.GetBindedOptions<CacheOptions>(configuration,
                        Constant.CacheOptionsKey);

                    cache.Set(Constant.GetUsersCacheKey, users,
                       new MemoryCacheEntryOptions()
                       {
                           Size = options.Size,
                           AbsoluteExpiration = DateTime.Now.AddMinutes(options.ExpirationInMinutes)
                       });
                }
            };

            return TypedResults.Ok(users);
        }

        public async Task<Ok<UserViewModel>> GetUserAsync(int id)
        {
            var user = context.User.FirstOrDefault(p => p.Id == id);

            if (user is null)
            {
                return TypedResults.Ok(new UserViewModel());
            }

            return TypedResults.Ok(mapper.Map<UserViewModel>(user));
        }

        public async Task<Ok> CreateUserAsync([FromBody] UserViewModel user)
        {
            await context.AddAsync(mapper.Map<User>(user));
            await context.SaveChangesAsync();

            return TypedResults.Ok();
        }
    }
}