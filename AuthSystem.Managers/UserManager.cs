using AuthSystem.Data;
using AuthSystem.Interfaces;
using System;
using System.Threading.Tasks;

namespace AuthSystem.Managers
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

    public class UserManager
    {
        private IUserAdapter Adapter { get; }

        public UserManager(IUserAdapter adapter)
        {
            Adapter = adapter;
        }

        public async Task<CreateUserResults> CreateUserAsync(string username, string password)
        {
            Guid id;
            do
            {
                id = Guid.NewGuid();
            } while (!await Adapter.IsUserIdUniqueAsync(id));

            if (!await Adapter.IsUsernameUniqueAsync(username))
            {
                return CreateUserResults.UsernameAlreadyExists;
            }

            var user = new User
            {
                Id = id,
                Username = username,
                HashedPassword = new HashedPassword
                {

                },
            };

            await Adapter.CreateAsync(user);

            return CreateUserResults.UserCreated;
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
