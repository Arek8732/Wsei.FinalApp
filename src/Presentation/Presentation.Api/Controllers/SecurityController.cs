using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Presentation.Api.Controllers;

public class JwtConfig
{
    public string Issuer { get; set; } = default!;
    public string Audience { get; set; } = default!;
    public string Key { get; set; } = default!;
}

public record WebApiUser(string UserName, string Password);

[ApiController]
[Route("api/[controller]")]
public class SecurityController : ControllerBase
{
    private readonly IOptions<JwtConfig> _config;
    public SecurityController(IOptions<JwtConfig> config) => _config = config;

    [AllowAnonymous]
    [HttpPost("token")]
    public IActionResult GenerateToken([FromBody] WebApiUser user)
    {
        if (AuthorizeUser(user.UserName, user.Password))
        {
            var issuer = _config.Value.Issuer;
            var audience = _config.Value.Audience;
            var key = Encoding.ASCII.GetBytes(_config.Value.Key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id", Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Email, user.UserName),
                    new Claim(ClaimTypes.Role, user.UserName == "admin" ? "Admin" : "User"),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(60),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var stringToken = tokenHandler.WriteToken(token);
            return Ok(new { access_token = stringToken });
        }
        return Unauthorized();
    }

    private static bool AuthorizeUser(string userName, string password)
        => (userName == "admin" || userName == "user") && password == "wsei";
}
