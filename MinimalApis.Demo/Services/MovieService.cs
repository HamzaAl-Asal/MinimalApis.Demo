using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using MinimalApis.Demo.Context;
using MinimalApis.Demo.Context.Models.Movies;
using MinimalApis.Demo.Interfaces;
using MinimalApis.Demo.Models.Movies;
using MinimalApis.Demo.Options;
using MinimalApis.Demo.Options.Extentions;

namespace MinimalApis.Demo.Services
{
    public class MovieService : IMovieService
    {
        private readonly IMemoryCache cache;
        private readonly ILogger<MovieService> logger;
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;
        private readonly MinimalApisContext context;

        public MovieService(IMemoryCache cache,
            ILogger<MovieService> logger,
            IMapper mapper,
            IConfiguration configuration,
            MinimalApisContext context)
        {
            this.cache = cache;
            this.logger = logger;
            this.mapper = mapper;
            this.configuration = configuration;
            this.context = context;
        }

        public async Task<Ok<IEnumerable<MovieViewModel>>> GetMoviesAsync()
        {
            if (cache.TryGetValue(Constant.GetMoviesCacheKey, out IEnumerable<MovieViewModel> movies))
            {
                logger.LogInformation("Found Movies List in cache");
            }
            else
            {
                logger.LogInformation("No Movies List in cache..");
                logger.LogInformation("Fetching movies from the database..");

                movies = mapper.Map<IEnumerable<MovieViewModel>>(context.Movie.ToList());

                if (movies.Any())
                {
                    var options = OptionBinding.GetBindedOptions<CacheOptions>(configuration,
                        Constant.CacheOptionsKey);

                    cache.Set(Constant.GetMoviesCacheKey,
                        movies,
                        new MemoryCacheEntryOptions()
                        {
                            Size = options.Size,
                            AbsoluteExpiration = DateTime.Now.AddMinutes(options.ExpirationInMinutes)
                        });
                }
            };

            return TypedResults.Ok(movies);
        }

        public async Task<Ok<MovieViewModel>> GetMovieAsync(int id)
        {
            var movie = context.Movie.FirstOrDefault(p => p.Id == id);

            if (movie is null)
            {
                return TypedResults.Ok(new MovieViewModel());
            }

            return TypedResults.Ok(mapper.Map<MovieViewModel>(movie));
        }

        public async Task<Ok> CreateMovieAsync([FromBody] MovieViewModel movie)
        {
            await context.AddAsync(mapper.Map<Movie>(movie));
            await context.SaveChangesAsync();

            return TypedResults.Ok();
        }
    }
}