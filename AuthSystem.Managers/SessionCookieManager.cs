using AuthSystem.Data;
using AuthSystem.Interfaces.Adapters;
using AuthSystem.Interfaces.Managers;
using System;
using System.Threading.Tasks;

namespace AuthSystem.Managers
{
    public class SessionCookieManager : ISessionCookieManager
    {
        private ISessionCookieAdapter Adapter { get; }
        private IUserManager UserManager { get; }

        public SessionCookieManager(ISessionCookieAdapter adapter, IUserManager userManager)
        {
            Adapter = adapter;
            UserManager = userManager;
        }

        public async Task<SessionCookieId> CreateSessionCookieAsync(Username username)
        {
            var sessionId = new SessionCookieId(Guid.NewGuid());
            var userId = await UserManager.GetIdForUsernameAsync(username);
            if (!userId.HasValue)
            {
                // TODO - what to do here? user ID should be present if they logged in successfully
                throw new Exception("No user ID found for username");
            }

            // TODO - check if numCreated == 1, throw exception if it's not?
            var numCreated = await Adapter.CreateSessionCookieAsync(new SessionCookie(sessionId, userId.Value));
            return sessionId;
        }

        public async Task<UserId?> GetUserForSessionAsync(SessionCookieId cookieId)
        {
            return await Adapter.GetUserForCookieAsync(cookieId);
        }

        public async Task DeleteSessionsForUserAsync(UserId userId)
        {
            await Adapter.DeleteSessionsForUserAsync(userId);
        }
    }
}
