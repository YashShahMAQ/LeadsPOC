using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GetLeadsMicroService.Utilities
{
    public class JwtTokenGenerator
    {
        public static string GenerateToken(string secretKey, string issuer, string audience, int expiryMinutes = 60)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
                signingCredentials: creds
                //claims: new[]
                //{
                //    new Claim(ClaimTypes.Name, "TestUser"), // Add any additional claims here
                //    new Claim(ClaimTypes.Role, "User")      // Example claim for roles
                //}
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
