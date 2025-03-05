using Application.Features.Items.Update;
using MediatR;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Items
{
    public class Update : IEndpoint
    {
        public sealed class Request
        {
            public string Barcode { get; set; } = string.Empty;
            public string Name { get; set; } = string.Empty;
            public string Descriptions { get; set; } = string.Empty;
            public decimal UnitPrice { get; set; }
        }

        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPut("items/{id:guid}", async (Guid id, Request request, ISender sender, CancellationToken cancellationToken) =>
            {
                var command = new UpdateItemCommand
                (
                    id,
                    request.Barcode,
                    request.Name,
                    request.Descriptions,
                    request.UnitPrice
                );

                Result result = await sender.Send(command, cancellationToken);

                return result.Match(Results.NoContent, CustomResults.Problem);

            }).WithTags(Tags.Items)
            .RequireAuthorization();
        }
    }
}
