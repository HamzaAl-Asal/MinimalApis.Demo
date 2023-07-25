using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MinimalApis.Demo.Models.Users;

namespace MinimalApis.Demo.Interfaces
{
    public interface IUserService
    {
        Task<Ok<IEnumerable<UserViewModel>>> GetUsersAsync();
        Task<Ok<UserViewModel>> GetUserAsync(int id);
        Task<Ok> CreateUserAsync([FromBody] UserViewModel user);
    }
}