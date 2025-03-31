
using Microsoft.AspNetCore.Mvc;

using BookManager.Dtos;
using BookManager.Services;
using BookManager.Data;

namespace BookManager.Endpoints
{
    public static class UserEndpoint
    {
        public static RouteGroupBuilder UseUserEndpoint(this WebApplication app)
        {
            RouteGroupBuilder group = app.MapGroup("users");

            group.MapPost("/register", ([FromBody] RegisterDto body, BookManagerContext dbContext) =>
            {
            });

            group.MapPost("/login", ([FromBody] LoginDto body, JWTService jwt) =>
            {
                if (body.Username == "admin" && body.Password == "password")
                {
                    var token = jwt.GenerateJwtToken(body.Username);
                    return Results.Ok(new { Token = token });
                }

                return Results.Unauthorized();
            });

            group.MapPost("/logout", () => { });

            return group;
        }
    }
}