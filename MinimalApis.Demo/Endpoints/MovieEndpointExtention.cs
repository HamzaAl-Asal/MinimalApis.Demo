using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Caching.Memory;
using MinimalApis.Demo.Models.Movies;
using MinimalApis.Demo.Options;

namespace MinimalApis.Demo.Endpoints
{
    public static class MovieEndpointExtention
    {
        public static void MapMovieEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("api/movies", HandleGetMoviesAsync)
                .WithSummary("Get all movies")
                .WithTags("Movie Apis");
        }

        private static async Task<Ok<IEnumerable<MovieViewModel>>> HandleGetMoviesAsync(IMemoryCache cache,
            ILogger<MovieViewModel> logger,
            IConfiguration configuration,
            CancellationToken cancellationToken)
        {
            if (cache.TryGetValue(Constant.GetMoviesCacheKey, out IEnumerable<MovieViewModel> movies))
            {
                logger.LogInformation("Found Movies List in cache");
            }
            else
            {
                var options = new CacheOptions();
                configuration.GetSection(Constant.CacheOptionsKey).Bind(options);

                movies = GetMoviesSeedData();

                cache.Set(Constant.GetMoviesCacheKey, movies,
                    new MemoryCacheEntryOptions()
                    {
                        Size = options.Size,
                        AbsoluteExpiration = DateTime.Now.AddMinutes(options.ExpirationInMinutes)
                    });
            };

            return TypedResults.Ok(movies);
        }

        private static IEnumerable<MovieViewModel> GetMoviesSeedData()
        {
            return new List<MovieViewModel>()
            {
                new MovieViewModel {
                    Id = 1,
                    Name = "Harry Potter",
                    Description = "",
                    Rate = 10
                },
                new MovieViewModel {
                    Id = 2,
                    Name = "12 Angry Men",
                    Description = "",
                    Rate = 9
                },
                new MovieViewModel {
                    Id = 3,
                    Name = "The Shawshank Redemption",
                    Description = "The Shawshank Redemption Description",
                    Rate = 8.5
                },
                new MovieViewModel {
                    Id = 4,
                    Name = "The GodFather",
                    Description = "",
                    Rate = 8
                }
            };
        }
    }
}