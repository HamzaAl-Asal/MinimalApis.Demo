using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Caching.Memory;
using MinimalApis.Demo.Models.Users;
using MinimalApis.Demo.Options;

namespace MinimalApis.Demo.Endpoints
{
    public static class UserEndpointExtention
    {
        public static void MapUserEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("api/users", HandleGetUsersAsync)
                .WithSummary("Get all users")
                .WithTags("User Apis");

            app.MapGet("api/user/{id}", HandleGetUserAsync)
                .WithSummary("Get user by Id")
                .WithTags("User Apis");
        }

        private static async Task<Ok<IEnumerable<UserViewModel>>> HandleGetUsersAsync(IMemoryCache cache,
            ILogger<UserViewModel> logger,
            IConfiguration configuration,
            CancellationToken cancellationToken)
        {
            if (cache.TryGetValue(Constant.GetUsersCacheKey, out IEnumerable<UserViewModel> users))
            {
                logger.LogInformation("Found Users List in cache");
            }
            else
            {
                users = GetUsersSeedData();

                var options = new CacheOptions();
                configuration.GetSection(Constant.CacheOptionsKey)
                    .Bind(options);

                cache.Set(Constant.GetUsersCacheKey, users,
                    new MemoryCacheEntryOptions()
                    {
                        Size = options.Size,
                        AbsoluteExpiration = DateTime.Now.AddMinutes(options.ExpirationInMinutes)
                    });
            };

            return TypedResults.Ok(users);
        }

        private static async Task<Ok<UserViewModel>> HandleGetUserAsync(int id,
            CancellationToken cancellationToken)
        {
            var user = GetUsersSeedData()
                .FirstOrDefault(p => p.Id == id);

            if (user is null)
            {
                return TypedResults.Ok(new UserViewModel());
            }

            return TypedResults.Ok(user);
        }

        private static IEnumerable<UserViewModel> GetUsersSeedData()
        {
            return new List<UserViewModel>()
            {
                new UserViewModel {
                    Id = 1,
                    Name = "Test",
                    Age = 25,
                    Job = "Fresh Software Developer"
                },
                new UserViewModel {
                    Id = 2,
                    Name = "Test2",
                    Age = 26,
                    Job = "Junior Software Developer"
                },
                new UserViewModel {
                    Id = 3,
                    Name = "Test3",
                    Age = 27,
                    Job = "Mid-Level Software Developer"
                },
                new UserViewModel {
                    Id = 4,
                    Name = "Test4",
                    Age = 28,
                    Job = "Senior Software Developer"
                }
            };
        }
    }
}