using AuthSystem.Data;
using AuthSystem.Interfaces;
using AuthSystem.Interfaces.Adapters;
using AuthSystem.Interfaces.Managers;
using OneOf;
using System;
using System.Threading.Tasks;

namespace AuthSystem.Managers
{
    public class UserManager : IUserManager
    {
        private IUserAdapter Adapter { get; }
        private IPasswordService PasswordService { get; }

        public UserManager(IUserAdapter adapter, IPasswordService passwordService)
        {
            Adapter = adapter;
            PasswordService = passwordService;
        }

        public async Task<bool> ValidatePasswordAsync(string username, string password)
        {
            var user = await Adapter.GetUserByUsernameAsync(username);
            if (!user.HasValue)
            {
                return false;
            }

            return PasswordService.CheckIfPasswordMatchesHash(password, user.Value.HashedPassword);
        }

        public async Task<OneOf<UsernameAlreadyExists, UserCreated>> CreateUserAsync(string username, string password)
        {
            var id = Guid.NewGuid();

            if (!await Adapter.IsUsernameUniqueAsync(username))
            {
                return new UsernameAlreadyExists();
            }

            var user = new User(id, username, PasswordService.GeneratePasswordHashAndSalt(password));

            await Adapter.CreateAsync(user);

            return UserCreated.From(id);
        }

        public async Task<ChangePasswordResult> ChangePasswordAsync(Guid userId, string oldPassword, string newPassword)
        {
            var existingUser = await Adapter.GetUserByIdAsync(userId);
            if (!existingUser.HasValue)
            {
                return ChangePasswordResult.UserNotPresent;
            }

            var isOldPasswordCorrect =
                PasswordService.CheckIfPasswordMatchesHash(oldPassword, existingUser.Value.HashedPassword);
            if (!isOldPasswordCorrect)
            {
                return ChangePasswordResult.PasswordIncorrect;
            }

            var newPasswordHash = PasswordService.GeneratePasswordHashAndSalt(newPassword);
            var newUserData = new User(existingUser.Value.Id, existingUser.Value.Username, newPasswordHash);
            
            // TODO - check if return == 1, throw error if not?
            await Adapter.UpdateAsync(newUserData);

            return ChangePasswordResult.PasswordChanged;
        }

        public async Task<OneOf<UsernameDoesNotExist, UserIdReturned>> GetIdForUsername(string username)
        {
            var user = await Adapter.GetUserByUsernameAsync(username);
            if (user.HasValue)
            {
                return UserIdReturned.From(user.Value.Id);
            }
            else
            {
                return new UsernameDoesNotExist();
            }
        }

    }
}
