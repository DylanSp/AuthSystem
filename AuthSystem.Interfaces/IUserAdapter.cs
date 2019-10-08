using System;
using AuthSystem.Data;
using System.Threading.Tasks;

namespace AuthSystem.Interfaces
{
    public interface IUserAdapter
    {
        Task<User?> GetUserByIdAsync(Guid userId);
        Task<User?> GetUserByUsernameAsync(string username);
        Task CreateAsync(User newUser);
        Task<int> UpdateAsync(User newUser);
        Task<bool> IsUsernameUniqueAsync(string username);
    }
}
