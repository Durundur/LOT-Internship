using LOT_TASK.Dtos;
using LOT_TASK.Models;
using LOT_TASK.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace LOT_TASK.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<UserModel> _userManager;
        private readonly IConfiguration _configuration;

        public AuthService(UserManager<UserModel> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<AuthResponseDto> Login(AuthRequestDto credentials)
        {
            var user = await _userManager.FindByEmailAsync(credentials.Email);

            if (user != null && await _userManager.CheckPasswordAsync(user, credentials.Password))
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                var response = new AuthResponseDto()
                {
                    JwtToken = this.GenerateTokenString(user.Email, userRoles),
                    RefreshToken = this.GenerateRefreshTokenString(),
                    Success = true,
                    Message = "Successfully signed in."
                };
                user.RefreshToken = response.RefreshToken;
                user.RefreshTokenExpiry = DateTime.Now.AddMinutes(Double.Parse(_configuration.GetSection("Jwt:RefreshTokenExpirationTimeMinutes").Value));
                await _userManager.UpdateAsync(user);
                return response;
            }
            return new AuthResponseDto()
            {
                Success = false,
                Message = "Login failed, wrong credentials",
            };
        }

        public async Task<AuthResponseDto> Register(AuthRequestDto credentials)
        {
            var user = new UserModel
            {
                UserName = credentials.Email,
                Email = credentials.Email
            };
            var userResult = await _userManager.CreateAsync(user, credentials.Password);
            if (userResult.Succeeded)
            {
                var response = new AuthResponseDto()
                {
                    JwtToken = this.GenerateTokenString(user.Email, new List<string>{ "user" }),
                    RefreshToken = this.GenerateRefreshTokenString(),
                    Success = true,
                    Message = "Successfully registered."
                };
                user.RefreshToken = response.RefreshToken;
                user.RefreshTokenExpiry = DateTime.Now.AddMinutes(Double.Parse(_configuration.GetSection("Jwt:RefreshTokenExpirationTimeMinutes").Value));
                await _userManager.UpdateAsync(user);
                return response;
            }
            return new AuthResponseDto
            {
                Success = false,
                Message = "Registration failed"
            };
        }

        public async Task<AuthResponseDto> RefreshToken(RefreshTokenRequest jwt)
        {
            var principal = GetTokenPrincipal(jwt.JwtToken);

            var response = new AuthResponseDto
            {
                Success = false,
            };
            if (principal?.Identity?.Name is null)
                return response;

            var user = await _userManager.FindByNameAsync(principal.Identity.Name);

            if (user is null || user.RefreshToken != jwt.RefreshToken || user.RefreshTokenExpiry < DateTime.Now)
                return response;

            var userRoles = await _userManager.GetRolesAsync(user);
            response.JwtToken = this.GenerateTokenString(user.Email, userRoles);
            response.RefreshToken = this.GenerateRefreshTokenString();
            response.Success = true;

            user.RefreshToken = response.RefreshToken;
            user.RefreshTokenExpiry = DateTime.Now.AddMinutes(Double.Parse(_configuration.GetSection("Jwt:RefreshTokenExpirationTimeMinutes").Value));
            await _userManager.UpdateAsync(user);
            return response;
        }

        private ClaimsPrincipal? GetTokenPrincipal(string token)
        {

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("Jwt:Key").Value));

            var validation = new TokenValidationParameters
            {
                IssuerSigningKey = securityKey,
                ValidateLifetime = false,
                ValidateActor = false,
                ValidateIssuer = true,
                ValidateAudience = true,
            };
            return new JwtSecurityTokenHandler().ValidateToken(token, validation, out _);
        }

        private string GenerateRefreshTokenString()
        {
            var randomNumber = new byte[64];

            using (var numberGenerator = RandomNumberGenerator.Create())
            {
                numberGenerator.GetBytes(randomNumber);
            }

            return Convert.ToBase64String(randomNumber);
        }

        private string GenerateTokenString(string email, IList<string> roles)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email,email),
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var staticKey = _configuration.GetSection("Jwt:Key").Value;
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(staticKey));
            var signingCred = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            var securityToken = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(Double.Parse(_configuration.GetSection("Jwt:ExpirationTimeMinutes").Value)),
                signingCredentials: signingCred,
                issuer: _configuration.GetSection("Jwt:Issuer").Value
                );

            string tokenString = new JwtSecurityTokenHandler().WriteToken(securityToken);
            return tokenString;
        }
    }
}
