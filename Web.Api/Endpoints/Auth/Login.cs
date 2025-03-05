using Application.Features.Users.Login;
using MediatR;
using SharedKernel;

namespace Web.Api.Endpoints.Auth
{
    internal sealed class Login : IEndpoint
    {
        public sealed class Request
        {
            public string Email { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
        }

        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("users/login", async (Request request, ISender sender, CancellationToken cancellationToken) =>
            {
                var command = new LoginUserCommand
                {
                    Email = request.Email,
                    Password = request.Password
                };

                Result<LoginResult> result = await sender.Send(command, cancellationToken);

                if (result.IsSuccess)
                {
                    return Results.Ok(new { result.Value });
                }

                return Results.Problem("Invalid credentials", statusCode: 401);
            }).WithTags(Tags.Users);

        }
    }
}
