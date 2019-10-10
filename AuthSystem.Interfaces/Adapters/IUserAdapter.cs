using System;
using AuthSystem.Data;
using System.Threading.Tasks;

namespace AuthSystem.Interfaces.Adapters
{
    public interface IUserAdapter
    {
        Task<User?> GetUserByIdAsync(UserId userId);
        Task<User?> GetUserByUsernameAsync(Username username);
        Task CreateAsync(User newUser);
        Task<int> UpdateAsync(User newUser);
        Task<bool> IsUsernameUniqueAsync(Username username);
    }
}
