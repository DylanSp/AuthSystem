using AuthSystem.Data;
using AuthSystem.Interfaces;
using System;
using System.Threading.Tasks;

namespace AuthSystem.Managers
{
    public enum CreateUserResults
    {
        UserIdAlreadyExists,
        UsernameAlreadyExists,
        UserCreated,
    }

    public enum ChangePasswordResults
    {
        UserNotPresent,
        PasswordIncorrect,
        PasswordChanged,
    }

    public class UserManager
    {
        private IUserAdapter Adapter { get; }

        public UserManager(IUserAdapter adapter)
        {
            Adapter = adapter;
        }

        public async Task CreateUser(User user)
        {
            // need to check that neither user ID nor username already exist
        }

        public async Task ChangePassword(Guid userId, string oldPassword, string newPassword)
        {
            throw new NotImplementedException();
            // read user - if null, return USER_NOT_PRESENT
            // verify oldPassword against hash/salt - if fail, return PASSWORD_NOT_CORRECT
            // generate new salt, calculate hash, save new user, return PASSWORD_CHANGED
        }

    }
}
