using Application.Features.InvoicesLine.DeleteById;
using MediatR;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.InvoiceLine
{
    public class DeteteById : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapDelete("invoice-line/deleteby-invoiceid/{id:guid}", async (Guid id, ISender sender, CancellationToken cancellationToken) =>
            {
                var command = new DeleteInvoiceLineCommand(id);

                Result result = await sender.Send(command, cancellationToken);

                return result.Match(Results.NoContent, CustomResults.Problem);

            }).WithTags(Tags.InvoiceLines)
            .RequireAuthorization();
        }
    }
}
