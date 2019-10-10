using AuthSystem.Data;

namespace AuthSystem.Interfaces
{
    public interface IPasswordService
    {
        HashedPassword GeneratePasswordHashAndSalt(PlaintextPassword password);
        bool CheckIfPasswordMatchesHash(PlaintextPassword password, HashedPassword hash);
    }
}
