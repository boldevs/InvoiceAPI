
using Application.Features.Items.Get;
using MediatR;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Items
{
    public class GetAll : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("items", async (ISender sender, CancellationToken cancellationToken) =>
            {
                var command = new GetItemsQuery();

                Result<List<ItemsRespone>> result = await sender.Send(command, cancellationToken);

                return result.Match(Results.Ok, CustomResults.Problem);

            }).WithTags(Tags.Items)
            .RequireAuthorization();
        }
    }
}
