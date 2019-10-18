using AuthSystem.Data;
using System.Threading.Tasks;

namespace AuthSystem.Interfaces.Managers
{
    public enum ChangePasswordResult
    {
        UserNotPresent,
        PasswordIncorrect,
        PasswordChanged,
    }

    public interface IUserManager
    {
        Task<bool> ValidatePasswordAsync(Username username, PlaintextPassword password);
        Task<UserId?> CreateUserAsync(Username username, PlaintextPassword password);
        Task<ChangePasswordResult> ChangePasswordAsync(UserId userId, PlaintextPassword oldPassword, PlaintextPassword newPassword);
    }
}
