using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MinimalApis.Demo.Models.Movies;

namespace MinimalApis.Demo.Interfaces
{
    public interface IMovieService
    {
        Task<Ok<IEnumerable<MovieViewModel>>> GetMoviesAsync();
        Task<Ok<MovieViewModel>> GetMovieAsync(int id);
        Task<Ok> CreateMovieAsync([FromBody] MovieViewModel movie);
    }
}