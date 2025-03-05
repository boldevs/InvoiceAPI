using SharedKernel;

namespace Domain.Entities.Items
{
    public static class ItemError
    {
        public static Error NotFound(Guid itemId) => Error.NotFound(
            "Item.NotFound",
            $"The itemId with the Id = '{itemId}' was not found");

        public static readonly Error NotFoundByName = Error.NotFound(
            "Item.NotFoundByName",
            "The Item with the specified name was not found");

        public static readonly Error InvalidItemId = Error.Validation(
            "Item.InvalidItemId",
            "The provided Item Id is invalid");

        public static readonly Error DuplicateName = Error.Conflict(
            "Item.DuplicateName",
            "The provided Item Name is already in use by another Item");

        public static readonly Error DuplicateBarcode = Error.Conflict(
            "Item.DuplicateBarcode",
            "The provided Barcode is already in use by another Item");

        public static readonly Error DescriptionTooLong = Error.Validation(
            "Customer.DescriptionTooLong",
            "The provided Descriptions Item exceeds the maximum length allowed");

        // 🆕 New Error: Handle database failures
        public static Error DatabaseError(string details) => Error.Failure(
            "Item.DatabaseError",
            $"A database error occurred: {details}");
    }
}
