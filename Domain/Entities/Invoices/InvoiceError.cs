using SharedKernel;

namespace Domain.Entities.Invoices
{
    public static class InvoiceError
    {
        public static Error NotFound(Guid invoiceId) => Error.NotFound(
            "Invoices.NotFound",
            $"The invoiceId with the Id = '{invoiceId}' was not found");

        public static readonly Error NotFoundByInvoiceNumber = Error.NotFound(
            "Invoices.InvoiceNumber",
            "The Invoice Number with the specified name/Number was not found");

        public static readonly Error NotFoundCustomerName = Error.NotFound(
            "Invoices.NotFoundCustomerName",
            "The Customer Name with the specified Name was not found");

        public static readonly Error InvoiceNumberNotUnique = Error.Conflict(
            "Invoices.InvoiceNumberNotUnique",
            "The provided Invoice Number is not unique");

        public static Error DatabaseError(string details) => Error.Failure(
            "Invoices.DatabaseError",
            $"A database error occurred: {details}");

    }
}
