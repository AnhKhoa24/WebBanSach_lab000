using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Metadata;
using System.Security.Claims;
using System.Text;

namespace HuynhKom_lab00_bansach.Middleware
{
    public class TokenValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        public TokenValidationMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            var endpoint = context.GetEndpoint();

            if (endpoint == null)
            {
                Console.WriteLine("Endpoint is null.");
                await _next(context);
                return;
            }

            var hasCustomAttribute = endpoint.Metadata.GetMetadata<QuanLyAttribute>() != null;
            Console.WriteLine($"Endpoint: {endpoint.DisplayName}, Has QuanLyAttribute: {hasCustomAttribute}");

            if (hasCustomAttribute)
            {
                if (context.Request.Cookies.TryGetValue("jwtTokenAdmin", out var token))
                {
                    var handler = new JwtSecurityTokenHandler();
                    var validationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])),
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidIssuer = _configuration["Jwt:Issuer"],
                        ValidAudience = _configuration["Jwt:Audience"],
                        ClockSkew = TimeSpan.Zero
                    };

                    try
                    {
                        var principal = handler.ValidateToken(token, validationParameters, out var validatedToken);

                        if (validatedToken is JwtSecurityToken jwtToken && jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                        {
                            context.User = principal;

                            var userRole = context.User.FindFirst(ClaimTypes.Role)?.Value;
                            if (userRole     != null && userRole == "Admin")
                            {
                                await _next(context);
                                return;
                            }
                            else
                            {
                                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                                await context.Response.WriteAsync("Forbidden: You don't have the right role.");
                                return;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        await context.Response.WriteAsync("Unauthorized: Token validation failed." + e.Message);
                        return;
                    }
                }
                else
                {
                    //context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    //await context.Response.WriteAsync("Unauthorized: No token provided.");
                    //return;
                    context.Response.Redirect("/Admin/Login");
                    context.Response.StatusCode = StatusCodes.Status302Found; 
                }
            }

            await _next(context); 
        }


    }
}
