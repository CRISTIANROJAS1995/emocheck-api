namespace Domain.Interfaces.Repositories
{
    public interface IJwtRepository
    {
        string GenerateToken(string userID, string name, string email, List<string> roles);
    }
}
