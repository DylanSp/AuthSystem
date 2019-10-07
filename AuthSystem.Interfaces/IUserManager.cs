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
        Task<CreateUserResults> CreateUserAsync(string username, string password);
        Task<ChangePasswordResults> ChangePassword(Guid userId, string oldPassword, string newPassword);
    }
}
