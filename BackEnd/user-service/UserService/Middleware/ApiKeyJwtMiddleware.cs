using UserService.Infrastructure;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using UserService.Service.Interface;
using UserService.Service;

namespace UserService.Middleware
{
    public class ApiKeyJwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _config;
        private const string APIKEY = "x-api-key";
        public ApiKeyJwtMiddleware(RequestDelegate next, IConfiguration config)
        {
            _next = next;
            _config = config;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.HasValue && context.Request.Path.Value == "/")
            {
                context.Response.StatusCode = StatusCodes.Status200OK;
                await context.Response.WriteAsync("Welcome");
                return;
            }
            //Check API_KEY
            if (!context.Request.Headers.TryGetValue(APIKEY, out var extractedApiKey))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Api Key was not provided");
                return;
            }

            var apiKey = (_config["UseEnv"] ?? "0") == "0" ? _config["API_KEY"] : Environment.GetEnvironmentVariable("API_KEY");

            if (apiKey != extractedApiKey)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Unauthorized client");
                return;
            }

            //Check Token
            string token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last() ?? "";
            try
            {
                var user = ValidateJwtToken(token, _config["Authentication:SecretKey"]);
                context.Items["User"] = user;
            }
            catch
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Unauthorized: token's invalid");
                return;
            }

            await _next(context);
        }

        public UserDTO ValidateJwtToken(string token, string secretKey)
        {
            if (string.IsNullOrEmpty(token))
                return new UserDTO();

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var user = new UserDTO()
                {
                    Id = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value),
                    RoleId = int.Parse(jwtToken.Claims.First(x => x.Type == "role_id").Value)
                };

                return user;
            }
            catch
            {
                return new UserDTO();
            }
        }
    }
}
