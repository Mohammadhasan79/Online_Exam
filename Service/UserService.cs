using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OnlineExam.Common;
using OnlineExam.DbContext;
using OnlineExam.DTOs.AuthDTOs;
using OnlineExam.Entity;
using OnlineExam.ServiceInterfaces;

namespace OnlineExam.Service
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly OnlineExamDbContext _context;
        public UserService(UserManager<User> userManager, IConfiguration configuration,OnlineExamDbContext context)
        {
            _userManager = userManager;
            _configuration = configuration;
            _context = context;
        }
        public async Task<Result> RegisterAsync(RegisterDTO dto)
        {
            if (dto == null) return Result.Fail("Data Entry is NULL");

            var exist = await _userManager.FindByNameAsync(dto!.UniversityCode);

            if (exist != null) return Result.Fail("The user has already registered.");
            var user = new User
            {
                UniversityCode = dto.UniversityCode,
                UserName = dto.UniversityCode,
                FistName = dto.FirstName,
                LastName = dto.LastName,
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join(" | ", result.Errors.Select(e => e.Description));
                return Result.Fail(errors);
            }
            if (dto.UniversityCode.Contains("@"))
            {
                var roleResult = await _userManager.AddToRoleAsync(user, "Prof");
                if (!roleResult.Succeeded)
                {
                    var errors = string.Join(" | ", roleResult.Errors.Select(e => e.Description));
                    return Result.Fail(errors);
                }
            }
            else
            {
                var roleResult = await _userManager.AddToRoleAsync(user, "Student");
                if (!roleResult.Succeeded)
                {
                    var errors = string.Join(" | ", roleResult.Errors.Select(e => e.Description));
                    return Result.Fail(errors);
                }
            }
            return Result.Ok("User created successfully");
        }
        public async Task<Result<TokenResponse>> LoginAsync(LoginDTO dto)
        {
            if (dto == null) return Result<TokenResponse>.Fail("Data Entry is NULL");

            var user = await _userManager.FindByNameAsync(dto.UniversityCode);
            if(user == null) return Result<TokenResponse>.Fail("User Not Found");

            var checkPassword = await _userManager.CheckPasswordAsync(user, dto.Password);
            if (!checkPassword) return Result<TokenResponse>.Fail("Password Incorrect");

            var accessToken = await GenerateToken(user);
            var refreshToken = RefreshToken();

            var token = new RefreshToken
            {
                Token = refreshToken,
                UserId = user.Id,
                ExpireTime = DateTime.Now.AddDays(7)
            };
            await _context.RefreshToken.AddAsync(token);
            await _context.SaveChangesAsync();
            return Result<TokenResponse>.Ok(
                new TokenResponse
                {
                    Token = accessToken,
                    RefreshToken = refreshToken,
                },
                "Login Succesfuly");
        }

        public async Task<Result<TokenResponse>> RefreshToken(string refreshToken)
        {
            var userToken = await _context.RefreshToken.Include(u => u.User)
                .FirstOrDefaultAsync(a => a.Token == refreshToken);

            if (userToken == null)
                return Result<TokenResponse>.Fail("Refresh Token Invalid");

            if (userToken.IsRevoked)
                return Result<TokenResponse>.Fail("Refresh Token Revoked");

            if (userToken.ExpireTime < DateTime.Now)
                return Result<TokenResponse>.Fail("Refresh Token Expire");

            var newAccessToken = await GenerateToken(userToken.User!);
            var newRefreshToken = RefreshToken();

            var newRefToken = new RefreshToken
            {
                Token = newRefreshToken,
                UserId = userToken.UserId,
                ExpireTime = DateTime.UtcNow.AddDays(7)
            };

            userToken.IsRevoked = true;
            await _context.RefreshToken.AddAsync(newRefToken);
            await _context.SaveChangesAsync();

            return Result<TokenResponse>.Ok(
                new TokenResponse
                {
                    Token = newAccessToken,
                    RefreshToken = newRefreshToken
                });
        }



        private async Task<string> GenerateToken(User user)
        {
            var secret = _configuration["Jwt:Secret"];
            var roles = await _userManager.GetRolesAsync(user);
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];
            int expiry = int.Parse(_configuration["Jwt:ExpireMinutes"]!);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id),
                new Claim(ClaimTypes.Name,user.FistName),
            };
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer,
                audience,
                claims,
                expires : DateTime.UtcNow.AddMinutes(expiry),
                signingCredentials : credentials
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        private string RefreshToken()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
