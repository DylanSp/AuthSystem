using OneOf;
using System;
using System.Threading.Tasks;
using ValueOf;

namespace AuthSystem.Interfaces
{
    // CreateUserAsync() return types
    public class UsernameAlreadyExists { }
    public class UserCreated : ValueOf<Guid, UserCreated> { }


    public enum ChangePasswordResult
    {
        UserNotPresent,
        PasswordIncorrect,
        PasswordChanged,
    }

    public interface IUserManager
    {
        Task<bool> ValidatePasswordAsync(string username, string password);
        Task<OneOf<UsernameAlreadyExists, UserCreated>> CreateUserAsync(string username, string password);
        Task<ChangePasswordResult> ChangePasswordAsync(Guid userId, string oldPassword, string newPassword);
    }
}
