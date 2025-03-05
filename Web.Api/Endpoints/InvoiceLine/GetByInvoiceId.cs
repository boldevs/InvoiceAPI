using Application.Features.InvoicesLine.GetByInvoiceId;
using MediatR;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.InvoiceLine
{
    public class GetByInvoiceId : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("invoice-line/getby-invoiceid/{id:guid}", async (Guid id, ISender sender, CancellationToken cancellationToken) =>
            {
                var command = new GetInvoiceLineByIdQuery(id);

                Result<List<InvoiceLineResponse>> result = await sender.Send(command, cancellationToken);

                return result.Match(Results.Ok, CustomResults.Problem);

            }).WithTags(Tags.InvoiceLines)
            .RequireAuthorization();
        }
    }
}
