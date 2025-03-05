using Application.Features.Invoices.Get;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Invoices
{
    public class GetAll : IEndpoint
    {
        public sealed class GetInvoiceQueryParameters
        {
            public Guid? UserId { get; set; }
            public Guid? CustomerId { get; set; }
            public string? InvoiceNumber { get; set; }
            public int PageNumber { get; set; } = 1;
            public int PageSize { get; set; } = 10;
        }

        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("invoices", async (
                [FromServices] ISender sender,
                [AsParameters] GetInvoiceQueryParameters queryParams,
                CancellationToken cancellationToken) =>
            {
                if (queryParams.PageNumber <= 0 || queryParams.PageSize <= 0)
                {
                    return Results.BadRequest("PageNumber and PageSize must be greater than zero.");
                }

                var query = new GetInvoiceQuery(
                    queryParams.UserId,
                    queryParams.CustomerId,
                    queryParams.InvoiceNumber,
                    queryParams.PageNumber,
                    queryParams.PageSize);

                Result<PagedResult<InvoicesResponse>> result = await sender.Send(query, cancellationToken);

                return result.Match(Results.Ok, CustomResults.Problem);
            })
           .WithTags(Tags.Invoices)
           .RequireAuthorization();
        }
    }
}
