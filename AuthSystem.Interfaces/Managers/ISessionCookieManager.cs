using AuthSystem.Data;
using System.Threading.Tasks;

namespace AuthSystem.Interfaces.Managers
{
    public interface ISessionCookieManager
    {
        Task<SessionCookieId> CreateSessionCookieAsync(Username username);
        Task<UserId?> GetUserForSessionAsync(SessionCookieId cookieId);
        Task DeleteSessionsForUserAsync(UserId userId);
    }
}
