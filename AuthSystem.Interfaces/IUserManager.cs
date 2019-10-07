using System;
using System.Threading.Tasks;

namespace AuthSystem.Interfaces
{
    public enum CreateUserResults
    {
        UsernameAlreadyExists,
        UserCreated,
    }

    public enum ChangePasswordResults
    {
        UserNotPresent,
        PasswordIncorrect,
        PasswordChanged,
    }

    public interface IUserManager
    {
        Task<bool> ValidatePasswordAsync(string username, string password);

        // TODO - return user ID when successful
        Task<CreateUserResults> CreateUserAsync(string username, string password);
        Task<ChangePasswordResults> ChangePasswordAsync(Guid userId, string oldPassword, string newPassword);
    }
}
