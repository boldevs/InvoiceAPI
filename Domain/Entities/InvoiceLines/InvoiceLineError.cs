using SharedKernel;

namespace Domain.Entities.InvoiceLines
{
    public static class InvoiceLineError
    {
        public static Error NotFound(Guid invoiceLineId) => Error.NotFound(
            "InvoicesLine.NotFound",
            $"The invoiceLineId with the Id = '{invoiceLineId}' was not found");

        public static readonly Error NotFoundItemId = Error.NotFound(
            "InvoicesLine.NotFoundItemId",
            "The Item Id with the specified Id was not found");

        public static readonly Error NotFoundIvnoiceId = Error.NotFound(
            "InvoicesLine.NotFoundIvnoiceId",
            "The Invoice Id with the specified Id was not found");

        public static Error DatabaseError(string details) => Error.Failure(
            "InvoicesLine.DatabaseError",
            $"A database error occurred: {details}");
    }
}
