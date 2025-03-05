namespace Application.Abstractions.Authentication
{
    public interface ITokenProvider
    {
        string CreateToken(Guid userId, string email, string firstName, string lastName, IList<string> roles);
    }
}
