using AuthSystem.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AuthSystem.Interfaces.Adapters
{
    public interface ISessionCookieAdapter
    {
        Task<int> CreateSessionCookieAsync(SessionCookie cookie);
        Task<UserId?> GetUserForCookieAsync(SessionCookieId cookieId);
    }
}
