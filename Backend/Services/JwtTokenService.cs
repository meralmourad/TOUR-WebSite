using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;
using System;

public class JwtTokenService
{
    private readonly IConfiguration _configuration;

    public JwtTokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

       public string GenerateToken(string userId, string role)
    {
        var jwtSettings = _configuration.GetSection("Jwt");
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userId),
            new Claim(ClaimTypes.Role, role)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Issuer"],
            claims: claims,
            expires: DateTime.MaxValue, // Set to a far future date to make it effectively not expire
            signingCredentials: creds
        );
        Console.WriteLine("Token generated: " + token+ " Role: " + role);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public ClaimsPrincipal? ValidateToken(string token)
    {
        var jwtSettings = _configuration.GetSection("Jwt");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));

        var tokenHandler = new JwtSecurityTokenHandler();
        try
        {
            var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true, // Enable audience validation
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings["Issuer"],
                ValidAudience = jwtSettings["Audience"], // Ensure Audience matches your app's settings
                IssuerSigningKey = key
            }, out SecurityToken validatedToken);

            return principal;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Token validation failed: {ex.Message}");
            return null;
        }
    }

    public bool TryValidateToken(string token, out ClaimsPrincipal? principal)
    {
        principal = null;
        var jwtSettings = _configuration.GetSection("Jwt");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));

        var tokenHandler = new JwtSecurityTokenHandler();
        try
        {
            principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings["Issuer"],
                ValidAudience = jwtSettings["Issuer"], // Match audience with issuer as in GenerateToken
                IssuerSigningKey = key
            }, out SecurityToken validatedToken);

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Token validation failed: {ex.Message}");
            return false;
        }
    }
}
