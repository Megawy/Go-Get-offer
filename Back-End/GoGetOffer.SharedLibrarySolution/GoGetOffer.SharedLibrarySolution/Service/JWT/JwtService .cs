using GoGetOffer.SharedLibrarySolution.DTOs.Auth.User.UserProfile;
using GoGetOffer.SharedLibrarySolution.Interface.JWT;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GoGetOffer.SharedLibrarySolution.Service.JWT
{
    public class JwtService(IConfiguration configuration) : IJwtService
    {
        private readonly IConfiguration _configuration = configuration;

        public string GenerateAccessToken(UserDTO user)
        {
            var key = Encoding.UTF8.GetBytes(_configuration["Authentication:Key"]!);
            var securityKey = new SymmetricSecurityKey(key);
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
        {
            new ("emailAddress", user.Email!),
            new ("user", user.Id.ToString()!),
            new (ClaimTypes.Role, user.UserType!.ToString()),
            new ("companyName", user.CompanyName ?? "")
        };

            var token = new JwtSecurityToken(
                issuer: _configuration["Authentication:Issuer"],
                audience: _configuration["Authentication:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(15),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken(UserDTO user)
        {
            var key = Encoding.UTF8.GetBytes(_configuration["Authentication:Key"]!);
            var securityKey = new SymmetricSecurityKey(key);
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
        {
            new ("emailAddress", user.Email!),
            new ("user", user.Id.ToString()!),
            new (ClaimTypes.Role, user.UserType!.ToString()),
            new ("companyName", user.CompanyName ?? "")
        };

            var token = new JwtSecurityToken(
                issuer: _configuration["Authentication:Issuer"],
                audience: _configuration["Authentication:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
