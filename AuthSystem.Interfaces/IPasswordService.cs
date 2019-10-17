using AuthSystem.Data;

namespace AuthSystem.Interfaces
{
    public interface IPasswordService
    {
        SaltedHashedPassword GeneratePasswordHashAndSalt(PlaintextPassword password);
        bool CheckIfPasswordMatchesHash(PlaintextPassword password, SaltedHashedPassword hash);
    }
}
