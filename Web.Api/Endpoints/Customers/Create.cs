using Application.Features.Customers.Create;
using MediatR;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Customers
{
    internal sealed class Create : IEndpoint
    {
        public sealed class Request
        {
            public string? Email { get; set; }
            public string? Name { get; set; }
            public string? Phone { get; set; }
            public string? Address { get; set; }
        }

        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("customers", async (Request request, ISender sender, CancellationToken cancellationToken) =>
            {
                var command = new CreateCustomerCommand
                {
                    Email = request.Email,
                    Name = request.Name,
                    Phone = request.Phone,
                    Address = request.Address
                };

                Result<Guid> result = await sender.Send(command, cancellationToken);

                return result.Match(Results.Ok, CustomResults.Problem);

            }).WithTags(Tags.Customers)
            .RequireAuthorization();
        }
    }
}
