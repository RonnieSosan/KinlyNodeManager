using KinlyNodeManagerService.Utility;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace KinleyNodeManager.API.Provider
{
    public static class AuthProvider
    {

        private const string Secret = "db3OIsj+BXE9NZDy0t8W3TcNekrF+2d/1sFnWG4HnV8TZY30iTOdtVWJG8abWvB1GlOgJuQZdcF2Luqm/hccMw==";

        public static ClaimsPrincipal GetPrincipal(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

                if (jwtToken == null)
                    return null;

                var symmetricKey = Convert.FromBase64String(Secret);

                var validationParameters = new TokenValidationParameters()
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(symmetricKey)
                };

                SecurityToken securityToken;
                var principal = tokenHandler.ValidateToken(token, validationParameters, out securityToken);

                return principal;
            }
            catch (SecurityTokenExpiredException expiredToken)
            {
                throw expiredToken;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool ValidateToken(string token, out ClaimsIdentity identity)
        {
            identity = null;
            try
            {
                var simplePrinciple = GetPrincipal(token);
                identity = simplePrinciple.Identity as ClaimsIdentity;

                if (identity == null)
                    return false;

                if (!identity.IsAuthenticated)
                    return false;
            }
            catch (SecurityTokenExpiredException expiredToken)
            {
                Logger.LogError(expiredToken);
                throw expiredToken;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                throw ex;
            }

            // More validate to check whether username exists in system

            return true;
        }

        public static Task<IPrincipal> AuthenticateJwtToken(string token)
        {
            ClaimsIdentity account;

            if (ValidateToken(token, out account))
            {
                // based on username to get more information from database in order to build local identity
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Sid, account.FindFirst(ClaimTypes.Sid)?.Value)
                    // Add more claims if needed: Roles, ...
                };

                var identity = new ClaimsIdentity(claims, "Jwt");
                IPrincipal user = new ClaimsPrincipal(identity);

                return Task.FromResult(user);
            }

            return Task.FromResult<IPrincipal>(null);
        }
    }
}
