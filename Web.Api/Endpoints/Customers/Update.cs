using Application.Features.Customers.Update;
using MediatR;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Customers
{
    internal sealed class Update : IEndpoint
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
            app.MapPut("customers/{id:guid}", async (Guid id, Request request, ISender sender, CancellationToken cancellationToken) =>
            {
                var command = new UpdateCustomerCommand(
                    id,
                    request.Email!,
                    request.Name!,
                    request.Phone!,
                    request.Address!
                );

                Result result = await sender.Send(command, cancellationToken);

                return result.Match(Results.NoContent, CustomResults.Problem);

            }).WithTags(Tags.Customers)
            .RequireAuthorization();
        }


    }
}
