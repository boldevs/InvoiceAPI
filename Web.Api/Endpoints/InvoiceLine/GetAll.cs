using Application.Features.InvoicesLine.Get;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.InvoiceLine
{
    public class GetAll : IEndpoint
    {
        public sealed class GetQueryParameters
        {
            public Guid? InvoiceId { get; set; }
            public Guid? ItemId { get; set; }
            public int PageNumber { get; set; } = 1;
            public int PageSize { get; set; } = 10;
        }

        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("invoice-line", async (
                [FromServices] ISender sender,
                [AsParameters] GetQueryParameters queryParams,
                CancellationToken cancellationToken) =>
            {
                if (queryParams.PageNumber <= 0 || queryParams.PageSize <= 0)
                {
                    return Results.BadRequest("PageNumber and PageSize must be greater than zero.");
                }

                var query = new GetInvoiceLineQuery(
                    queryParams.InvoiceId,
                    queryParams.ItemId,
                    queryParams.PageNumber,
                queryParams.PageSize);

                Result<PagedResult<InvoiceLineReponse>> result = await sender.Send(query, cancellationToken);

                return result.Match(Results.Ok, CustomResults.Problem);
            })
           .WithTags(Tags.InvoiceLines)
           .RequireAuthorization();
        }
    }
}
