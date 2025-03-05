using Application.Features.Items.GetById;
using MediatR;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Items
{
    public class GetById : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("items/{id}", async (ISender sender, Guid id, CancellationToken cancellationToken) =>
            {
                var command = new GetItemByIdQuery(id);

                Result<ItemResponse> result = await sender.Send(command, cancellationToken);

                return result.Match(Results.Ok, CustomResults.Problem);

            }).WithTags(Tags.Items)
            .RequireAuthorization();
        }
    }
}
