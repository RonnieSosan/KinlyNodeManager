using KinlyNodeManagerService.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
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
    public class TokenAuthenticator : Attribute, IAuthorizationFilter
    {
        public const string AuthenticationScheme = "Token";

        public string Realm { get; set; }
        public bool AllowMultiple => false;

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var token = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var request = context.HttpContext.Request;

            if (token == null)
            {// not logged in
                context.Result = new JsonResult(new { message = "Unauthorized: Missing Jwt Token" }) { StatusCode = StatusCodes.Status401Unauthorized };
                return;
            }

            var principal = AuthProvider.AuthenticateJwtToken(token);

            if (principal == null)
                context.Result = new JsonResult(new { message = "Invalid token" }) { StatusCode = StatusCodes.Status401Unauthorized };

            else
                context.HttpContext.User = (ClaimsPrincipal)principal.Result;
        }

        
    }
}
