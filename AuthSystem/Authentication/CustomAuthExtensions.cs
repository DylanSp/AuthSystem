using Microsoft.AspNetCore.Authentication;
using System;

namespace AuthSystem.Authentication
{
    public static class CustomAuthExtensions
    {
        public static AuthenticationBuilder AddCustomAuth(this AuthenticationBuilder builder,
            Action<CustomAuthOptions> configureOptions)
        {
            return builder.AddScheme<CustomAuthOptions, CustomAuthHandler>(Constants.CUSTOM_AUTH_SCHEME_NAME, Constants.CUSTOM_AUTH_SCHEME_DISPLAY_NAME, configureOptions);
        }
    }
}
