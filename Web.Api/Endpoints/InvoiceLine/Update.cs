using Application.Features.InvoicesLine.Update;
using MediatR;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.InvoiceLine
{
    public sealed class Request
    {
        public Guid InvoiceId { get; set; }
        public Guid ItemId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
    public class Update : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPut("invoice-line/{id}", async (Guid id, Request request, ISender sender, CancellationToken cancellationToken) =>
            {
                var command = new UpdateInvoiceLineUpdateCommand(
                    id,
                    request.ItemId,
                    request.Quantity,
                    request.UnitPrice
                );

                Result result = await sender.Send(command, cancellationToken);

                return result.Match(Results.NoContent, CustomResults.Problem);

            }).WithTags(Tags.InvoiceLines)
            .RequireAuthorization();
        }
    }
}
