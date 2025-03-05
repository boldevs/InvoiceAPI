using SharedKernel;

namespace Domain.Entities.Users
{
    public static class UserError
    {
        public static readonly Error InvalidEmail = Error.Validation(
            "User.InvalidEmailFormat",
            "The provided email format is invalid");

        public static readonly Error NotFound = Error.Validation(
            "User.UserNotFound",
            "The provided User ID is NotFound");
    }
}
