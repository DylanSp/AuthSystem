using System;
using AuthSystem.Data;
using System.Threading.Tasks;

namespace AuthSystem.Interfaces.Adapters
{
    public interface IUserAdapter
    {
        Task<User?> GetUserByIdAsync(UserId userId);
        Task<User?> GetUserByUsernameAsync(Username username);
        Task CreateUserAsync(User newUser);
        Task<int> UpdateUserAsync(User newUser);
        Task<bool> IsUsernameUniqueAsync(Username username);
    }
}
