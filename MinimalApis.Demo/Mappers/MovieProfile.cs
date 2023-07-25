using AutoMapper;
using MinimalApis.Demo.Context.Models.Movies;
using MinimalApis.Demo.Models.Movies;

namespace MinimalApis.Demo.Mappers
{
    public class MovieProfile : Profile
    {
        public MovieProfile()
        {
            CreateMap<Movie, MovieViewModel>()
                .ReverseMap();
        }
    }
}