using AuthSystem.Data;
using AuthSystem.Interfaces.Managers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace AuthSystem.Authentication
{
    internal class CustomAuthHandler : AuthenticationHandler<CustomAuthOptions>
    {
        private ISessionCookieManager SessionCookieManager { get; }

        public CustomAuthHandler(IOptionsMonitor<CustomAuthOptions> options, ILoggerFactory logger, UrlEncoder encoder,
            ISystemClock clock, ISessionCookieManager sessionCookieManager) : base(options, logger, encoder, clock)
        {
            SessionCookieManager = sessionCookieManager;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            Guid rawSessionCookieId;
            if (!Request.Cookies.TryGetValue(Constants.SESSION_COOKIE_NAME, out var sessionCookie)
                || !Guid.TryParse(sessionCookie, out rawSessionCookieId))
            {
                return AuthenticateResult.Fail("No valid session cookie attached");
            }

            var userId = await SessionCookieManager.GetUserForSessionAsync(new SessionCookieId(rawSessionCookieId));
            if (!userId.HasValue)
            {
                return AuthenticateResult.Fail("Session cookie invalid");
            }

            var claim = new Claim(Constants.USER_ID_CLAIM_NAME, userId.Value.Value.ToString());
            return AuthenticateResult.Success(new AuthenticationTicket(new ClaimsPrincipal(new ClaimsIdentity(new List<Claim> { claim }, Scheme.Name)), new AuthenticationProperties(), Scheme.Name));
        }
    }
}
