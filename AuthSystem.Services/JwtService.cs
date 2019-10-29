using AuthSystem.Data;
using AuthSystem.Interfaces;
using JWT;
using JWT.Algorithms;
using JWT.Builder;
using System;
using System.Collections.Generic;

namespace AuthSystem.Services
{
    public class JwtService : IJwtService
    {
        private JwtSecret Secret { get; }
        private const string USER_ID_CLAIM_NAME = "userId";

        public JwtService(JwtSecret secret)
        {
            Secret = secret;
        }

        public JsonWebToken CreateToken(UserId userId, DateTimeOffset expirationTime)
        {
            var token = new JwtBuilder()
                .WithAlgorithm(new HMACSHA512Algorithm())   // TODO - make this configurable?
                .WithSecret(Secret.Value)
                .AddClaim(ClaimName.ExpirationTime, expirationTime.ToUnixTimeSeconds().ToString())
                .AddClaim(USER_ID_CLAIM_NAME, userId.Value.ToString())
                .Build();
            return new JsonWebToken(token);
        }

        public UserId? DecodeToken(JsonWebToken token)
        {
            try
            {
                var json = new JwtBuilder()
                    .WithSecret(Secret.Value)
                    .MustVerifySignature()
                    .Decode<IDictionary<string, object>>(token.Value);
                Guid userId;
                if (!Guid.TryParse(json[USER_ID_CLAIM_NAME].ToString(), out userId))
                {
                    return null;
                }

                return new UserId(userId);
            }
            catch (TokenExpiredException)
            {
                return null;
            }
            catch (SignatureVerificationException)
            {
                return null;
            }

        }
    }
}
