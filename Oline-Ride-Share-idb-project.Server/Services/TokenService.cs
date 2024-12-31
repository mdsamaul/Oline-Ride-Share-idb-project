using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Oline_Ride_Share_idb_project.Server.Services
{
    public class TokenService : ITokenService
    {
        private string SecretKey { get; }
        private const string Issuer = "Online Ride Sharing";
        private const string Audience = "Isdb-BISEW";
        public TokenService()
        {
            SecretKey = GenerateSecretKey();
        }
        public string GenerateJwtToken(string phoneNumber, string userId)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, phoneNumber),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("userId", userId) 
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256); 
            var token = new JwtSecurityToken(
                issuer: Issuer,
                audience: Audience, 
                claims: claims, 
                expires: DateTime.Now.AddDays(1), 
                signingCredentials: creds); 

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        private string GenerateSecretKey()
        {
            var secretKey = Guid.NewGuid().ToString("N") + Guid.NewGuid().ToString("N");
            return secretKey.Substring(0, 32);
        }
    }
}
