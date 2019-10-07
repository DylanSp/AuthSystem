﻿using AuthSystem.Data;
using AuthSystem.Interfaces;
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
            throw new NotImplementedException();
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
                HashedPassword = PasswordService.GeneratePasswordHashAndSalt(password),
            };

            await Adapter.CreateAsync(user);

            return CreateUserResults.UserCreated;
        }

        public async Task<ChangePasswordResults> ChangePasswordAsync(Guid userId, string oldPassword, string newPassword)
        {
            var existingUser = await Adapter.GetUserByIdAsync(userId);
            if (!existingUser.HasValue)
            {
                return ChangePasswordResults.UserNotPresent;
            }

            var isOldPasswordCorrect =
                PasswordService.CheckIfPasswordMatchesHash(oldPassword, existingUser.Value.HashedPassword);
            if (!isOldPasswordCorrect)
            {
                return ChangePasswordResults.PasswordIncorrect;
            }

            var newPasswordHash = PasswordService.GeneratePasswordHashAndSalt(newPassword);
            var newUserData = new User
            {
                Id = existingUser.Value.Id,
                Username = existingUser.Value.Username,
                HashedPassword = newPasswordHash,
            };
            
            // TODO - check if return == 1, throw error if not?
            await Adapter.UpdateAsync(newUserData);

            return ChangePasswordResults.PasswordChanged;
        }

    }
}
