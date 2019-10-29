using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using AuthSystem.Data;
using AuthSystem.Interfaces;

namespace AuthSystem.Authentication
{
    internal class CustomAuthHandler : AuthenticationHandler<CustomAuthOptions>
    {
        private IJwtService JwtService { get; }

        public CustomAuthHandler(IOptionsMonitor<CustomAuthOptions> options, ILoggerFactory logger, UrlEncoder encoder,
            ISystemClock clock, IJwtService jwtService) : base(options, logger, encoder, clock)
        {
            // store custom services here... (request in ctor)
            JwtService = jwtService;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            string jwt;
            if (!Request.Cookies.TryGetValue("AccessToken", out jwt))
            {
                return AuthenticateResult.Fail("No AccessToken cookie attached");
            }

            var userId = JwtService.DecodeToken(new JsonWebToken(jwt));
            if (!userId.HasValue)
            {
                return AuthenticateResult.Fail("AccessToken invalid");
            }

            var claim = new Claim("UserId", userId.Value.Value.ToString());
            return AuthenticateResult.Success(new AuthenticationTicket(new ClaimsPrincipal(new ClaimsIdentity(new List<Claim> { claim }, Scheme.Name)),new AuthenticationProperties(), Scheme.Name));
            
            // build the claims and put them in "Context"; you need to import the Microsoft.AspNetCore.Authentication package
            // return AuthenticateResult.NoResult();
        }
    }
}
