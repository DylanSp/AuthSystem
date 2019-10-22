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

        public async Task<bool> ValidatePasswordAsync(Username username, PlaintextPassword password)
        {
            var user = await Adapter.GetUserByUsernameAsync(username);
            if (!user.HasValue)
            {
                return false;
            }

            return PasswordService.CheckIfPasswordMatchesHash(password, user.Value.SaltedHashedPassword);
        }

        public async Task<UserId?> CreateUserAsync(Username username, PlaintextPassword password)
        {
            var id = new UserId(Guid.NewGuid());
            var user = new User(id, username, PasswordService.GeneratePasswordHashAndSalt(password));

            var numInserted = await Adapter.CreateUserAsync(user);

            if (numInserted == 1)
            {
                return id;
            }

            // TODO - check numInserted > 1, throw exception in that case?
            return null;
        }

        public async Task<ChangePasswordResult> ChangePasswordAsync(UserId userId, PlaintextPassword oldPassword, PlaintextPassword newPassword)
        {
            var existingUser = await Adapter.GetUserByIdAsync(userId);
            if (!existingUser.HasValue)
            {
                return ChangePasswordResult.UserNotPresent;
            }

            var isOldPasswordCorrect =
                PasswordService.CheckIfPasswordMatchesHash(oldPassword, existingUser.Value.SaltedHashedPassword);
            if (!isOldPasswordCorrect)
            {
                return ChangePasswordResult.PasswordIncorrect;
            }

            var newPasswordHash = PasswordService.GeneratePasswordHashAndSalt(newPassword);
            var newUserData = new User(existingUser.Value.Id, existingUser.Value.Username, newPasswordHash);
            
            // TODO - check if return == 1, throw error if not?
            await Adapter.UpdateUserAsync(newUserData);

            return ChangePasswordResult.PasswordChanged;
        }
    }
}
