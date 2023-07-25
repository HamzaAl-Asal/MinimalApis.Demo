using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MinimalApis.Demo.Interfaces;
using MinimalApis.Demo.Models.Movies;

namespace MinimalApis.Demo.Endpoints
{
    public static class MovieEndpointExtention
    {
        public static void MapMovieEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("api/movies", HandleGetMoviesAsync)
                .WithSummary("Get all movies")
                .WithTags("Movie Apis");

            app.MapGet("api/movie/{id}", HandleGetMovieAsync)
                .WithSummary("Get movie")
                .WithTags("Movie Apis");

            app.MapPost("api/add-movie", HandleCreateMovieAsync)
                .WithSummary("Create movie")
                .WithTags("Movie Apis");
        }

        private static async Task<Ok<IEnumerable<MovieViewModel>>> HandleGetMoviesAsync(IMovieService movieService)
        {
            return await movieService.GetMoviesAsync();
        }

        private static async Task<Ok<MovieViewModel>> HandleGetMovieAsync(int id, IMovieService movieService)
        {
            return await movieService.GetMovieAsync(id);
        }

        private static async Task<Ok> HandleCreateMovieAsync([FromBody] MovieViewModel movie, IMovieService movieService)
        {
            return await movieService.CreateMovieAsync(movie);
        }
    }
}