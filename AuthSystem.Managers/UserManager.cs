using AuthSystem.Data;
using AuthSystem.Interfaces;
using System;
using System.Threading.Tasks;

namespace AuthSystem.Managers
{
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
        }

    }
}
