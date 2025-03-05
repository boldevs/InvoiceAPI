using Application.Features.Customers.Get;
using MediatR;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Customers
{
    public class GetAll : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("customers", async (ISender sender, CancellationToken cancellationToken) =>
            {
                var command = new GetCustomersQuery();

                Result<List<CustomerResponse>> result = await sender.Send(command, cancellationToken);

                return result.Match(Results.Ok, CustomResults.Problem);
            })
           .WithTags(Tags.Customers)
           .RequireAuthorization();
        }
    }
}
