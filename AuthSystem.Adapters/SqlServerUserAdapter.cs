using AuthSystem.Data;
using AuthSystem.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AuthSystem.Adapters
{
    public class SqlServerUserAdapter : IDataAdapter<User>
    {
        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<User>> ReadAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<(bool, User)> ReadAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task SaveAsync(User newData)
        {
            throw new NotImplementedException();
        }
    }
}
