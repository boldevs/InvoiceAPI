using Application.Features.Items.Create;
using MediatR;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Items
{
    public sealed class Create : IEndpoint
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
            app.MapPost("items", async (Request request, ISender sender, CancellationToken cancellationToken) =>
            {
                var command = new CreateItemCommand
                {
                    Barcode = request.Barcode,
                    Name = request.Name,
                    Descriptions = request.Descriptions,
                    UnitPrice = request.UnitPrice
                };

                Result<Guid> result = await sender.Send(command, cancellationToken);

                return result.Match(Results.Ok, CustomResults.Problem);

            }).WithTags(Tags.Items);
        }
    }
}
