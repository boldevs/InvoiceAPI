using Application.Features.Invoices.Create;
using MediatR;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Invoices
{
    public class Create : IEndpoint
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
            app.MapPost("invoices", async (Request request, ISender sender, CancellationToken cancellationToken) =>
            {
                var command = new CreateInvoiceCommand
                {
                    InvoiceNumber = request.InvoiceNumber,
                    UserId = request.UserId,
                    CustomerId = request.CustomerId,
                    IssuedDate = request.IssuedDate,
                    DueDate = request.DueDate,
                    TotalAmount = request.TotalAmount
                };

                Result<Guid> result = await sender.Send(command, cancellationToken);

                return result.Match(Results.Ok, CustomResults.Problem);

            }).WithTags(Tags.Invoices)
            .RequireAuthorization();
        }
    }
}
