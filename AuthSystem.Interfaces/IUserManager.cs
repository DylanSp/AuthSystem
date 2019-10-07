﻿using OneOf;
using System;
using System.Threading.Tasks;
using ValueOf;

namespace AuthSystem.Interfaces
{
    // CreateUserAsync() return types
    public class UsernameAlreadyExists { }
    public class UserCreated : ValueOf<Guid, UserCreated> { }


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
        Task<OneOf<UsernameAlreadyExists, UserCreated>> CreateUserAsync(string username, string password);
        Task<ChangePasswordResults> ChangePasswordAsync(Guid userId, string oldPassword, string newPassword);
    }
}
