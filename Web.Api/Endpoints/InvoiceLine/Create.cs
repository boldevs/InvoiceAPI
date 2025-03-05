using Application.Features.InvoicesLine.Create;
using MediatR;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.InvoiceLine
{
    public class Create : IEndpoint
    {
        public sealed class Request
        {
            public Guid InvoiceId { get; set; }
            public Guid ItemId { get; set; }
            public int Quantity { get; set; }
            public decimal UnitPrice { get; set; }
        }
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("invoice-line", async (Request request, ISender sender, CancellationToken cancellationToken) =>
            {
                var command = new CreateInvoiceLineCommand
                {
                    InvoiceId = request.InvoiceId,
                    ItemId = request.ItemId,
                    Quantity = request.Quantity,
                    UnitPrice = request.UnitPrice
                };

                Result<Guid> result = await sender.Send(command, cancellationToken);

                return result.Match(Results.Ok, CustomResults.Problem);

            }).WithTags(Tags.InvoiceLines)
            .RequireAuthorization();
        }
    }
}
