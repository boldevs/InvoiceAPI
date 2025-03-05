using SharedKernel;

namespace Domain.Entities.Customers
{
    public static class CustomerError
    {
        public static Error NotFound(Guid customerId) => Error.NotFound(
            "Customer.NotFound",
            $"Customer with ID '{customerId}' was not found");

        public static readonly Error NotFoundByName = Error.NotFound(
            "Customer.NotFoundByName",
            "No customer found with the specified name");

        public static readonly Error NotFoundByEmail = Error.NotFound(
            "Customer.NotFoundByEmail",
            "No customer found with the specified email");

        public static readonly Error EmailNotUnique = Error.Conflict(
            "Customer.EmailNotUnique",
            "The provided email is already associated with another customer");

        public static readonly Error InvalidEmailFormat = Error.Validation(
            "Customer.InvalidEmailFormat",
            "The provided email format is invalid. Please enter a valid email");

        public static readonly Error InvalidCustomerId = Error.Validation(
            "Customer.InvalidCustomerId",
            "The provided customer ID is invalid");

        public static readonly Error DuplicatePhoneNumber = Error.Conflict(
            "Customer.DuplicatePhoneNumber",
            "The provided phone number is already in use");

        public static readonly Error AddressTooLong = Error.Validation(
            "Customer.AddressTooLong",
            "The provided address exceeds the maximum allowed length");

        public static readonly Error PhoneNumberTooShort = Error.Validation(
            "Customer.PhoneNumberTooShort",
            "The provided phone number is too short");

        public static readonly Error PhoneNumberTooLong = Error.Validation(
            "Customer.PhoneNumberTooLong",
            "The provided phone number exceeds the maximum length of 15 digits");

        // 🆕 Standardized Database Error Handling
        public static readonly Func<string, Error> DatabaseError = details => Error.Failure(
            "Customer.DatabaseError",
            $"A database error occurred: {details}");
    }
}
