using Application.Features.Invoices.Update;
using MediatR;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Invoices
{
    public class Update : IEndpoint
    {
        public sealed class Request
        {
            public string InvoiceNumber { get; set; } = string.Empty;
            public Guid UserId { get; set; }
            public Guid CustomerId { get; set; }
            public DateTime DueDate { get; set; }
            public DateTime IssuedDate { get; set; }
            public decimal TotalAmount { get; set; }
        }
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPut("invoices/{id}", async (Guid id, Request request, ISender sender, CancellationToken cancellationToken) =>
            {
                var command = new UpdateInvoiceCommand(
                    id,
                    request.CustomerId,
                    request.UserId,
                    request.InvoiceNumber,
                    request.IssuedDate,
                    request.DueDate,
                    request.TotalAmount
                );

                Result result = await sender.Send(command, cancellationToken);

                return result.Match(Results.NoContent, CustomResults.Problem);

            }).WithTags(Tags.Invoices)
            .RequireAuthorization();
        }
    }
}
