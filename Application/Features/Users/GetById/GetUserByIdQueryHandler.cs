using Application.Abstractions.Authentication;
using Application.Abstractions.Messaging;
using Domain.Entities.Users;
using SharedKernel;

namespace Application.Features.Users.GetById
{
    public sealed class GetUserByIdQueryHandler(
        IUserService userService)
        : IQueryHandler<GetUserByIdQuery, UserResponse>
    {
        public async Task<Result<UserResponse>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await userService.GetUserByIdAsync(request.userId);

            if (user is null)
                return Result.Failure<UserResponse>(UserError.NotFound);

            var userResponse = new UserResponse
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
            };

            return Result.Success(userResponse);
        }
    }
}
