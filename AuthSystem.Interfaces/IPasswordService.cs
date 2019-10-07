using AuthSystem.Data;

namespace AuthSystem.Interfaces
{
    public interface IPasswordService
    {
        HashedPassword GeneratePasswordHashAndSalt(string password);
        bool CheckIfPasswordMatchesHash(string password, HashedPassword hash);
    }
}
