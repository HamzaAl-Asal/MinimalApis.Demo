using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MinimalApis.Demo.Interfaces;
using MinimalApis.Demo.Models.Users;

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

            app.MapPost("api/add-user", HandleCreateUserAsync)
               .WithSummary("Create User")
               .WithTags("User Apis");
        }

        private static async Task<Ok<IEnumerable<UserViewModel>>> HandleGetUsersAsync(IUserService userService)
        {
            return await userService.GetUsersAsync();
        }

        private static async Task<Ok<UserViewModel>> HandleGetUserAsync(int id,
            IUserService userService)
        {
            return await userService.GetUserAsync(id);
        }

        private static async Task<Ok> HandleCreateUserAsync([FromBody] UserViewModel user, IUserService userService)
        {
            return await userService.CreateUserAsync(user);
        }
    }
}