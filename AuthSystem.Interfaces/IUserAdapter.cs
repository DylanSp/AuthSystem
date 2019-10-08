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
        Task<int> UpdateAsync(User newUserData);    // TODO - make param name consistent
        Task<bool> IsUserIdUniqueAsync(Guid userId);
        Task<bool> IsUsernameUniqueAsync(string username);
    }
}
