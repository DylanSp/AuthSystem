using AuthSystem.Data;
using OneOf;
using System;
using System.Threading.Tasks;
using ValueOf;

namespace AuthSystem.Interfaces.Managers
{
    // CreateUserAsync() return types
    public class UsernameAlreadyExists { }
    public class UserCreated : ValueOf<UserId, UserCreated> { }


    public enum ChangePasswordResult
    {
        UserNotPresent,
        PasswordIncorrect,
        PasswordChanged,
    }

    // GetIdForUsername() return types
    public class UsernameDoesNotExist { }
    public class UserIdReturned : ValueOf<UserId, UserIdReturned> { }

    public interface IUserManager
    {
        Task<bool> ValidatePasswordAsync(Username username, PlaintextPassword password);
        Task<OneOf<UsernameAlreadyExists, UserCreated>> CreateUserAsync(Username username, PlaintextPassword password);
        Task<ChangePasswordResult> ChangePasswordAsync(UserId userId, PlaintextPassword oldPassword, PlaintextPassword newPassword);
        Task<OneOf<UsernameDoesNotExist, UserIdReturned>> GetIdForUsername(Username username);
    }
}
