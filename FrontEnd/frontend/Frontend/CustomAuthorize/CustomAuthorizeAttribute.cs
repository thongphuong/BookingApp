using Frontend.Models;
using Frontend.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace Frontend.CustomAuthorize
{
    public class CustomAuthorizeAttribute : TypeFilterAttribute
    {
        public CustomAuthorizeAttribute() : base(typeof(CustomAuthorizeFilter))
        {
        }
    }
    public class CustomAuthorizeFilter : IAuthorizationFilter
    {
        //private string _userInfoPrex = "Auth.UserInfo.{0}";
        private readonly IConfiguration _config;

        public CustomAuthorizeFilter(IConfiguration config)
        {
            _config = config;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var Secret = _config["Authentication:SecretKey"].ToString();
            string? token = context.HttpContext.Session.GetString("SessionToken");

            var jwtToken = GetTokenInfo(token, Secret);
            if (jwtToken == null)
            {
                context.Result = new RedirectResult("~/Authenticate");
                return;
            }
            var user = context.HttpContext.Session.Get<CurrentUserModel>("UserInfo");
            if (user == null)
            {
                context.Result = new RedirectResult("~/Authenticate");
                return;
            }

            
            //var identity = new ClaimsIdentity(user.UserName);
            //context.HttpContext.User = new ClaimsPrincipal(identity);
            //var IsAuthenticated = context.HttpContext.User?.Identity?.IsAuthenticated;
            //if (!(IsAuthenticated ?? false))
            //{
            //    if (context.HttpContext.Request.Headers["x-requested-with"] == "XMLHttpRequest")
            //    {
            //        context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized; //Set HTTP 401                         
            //    }
            //    context.Result = new RedirectResult("~/Authenticate");
            //}

            return;
        }
        public static bool IsTokenValid(string key, string issuer, string token)
        {
            var mySecret = Encoding.UTF8.GetBytes(key);
            var mySecurityKey = new SymmetricSecurityKey(mySecret);
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                tokenHandler.ValidateToken(token,
                new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = issuer,
                    ValidAudience = issuer,
                    ClockSkew = TimeSpan.Zero,
                    IssuerSigningKey = mySecurityKey,
                }, out SecurityToken validatedToken);
            }
            catch
            {
                return false;
            }
            return true;
        }
        public static JwtSecurityToken? GetTokenInfo(string? token, string secret)
        {
            SecurityToken validatedToken;
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(secret);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out validatedToken);
                return (JwtSecurityToken)validatedToken;
            }
            catch (Exception)
            {
                return null;
            }

        }
    }
}
