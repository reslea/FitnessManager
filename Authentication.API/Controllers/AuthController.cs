using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Authentication.API.Models;
using Authentication.Data;
using Authentication.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Authentication.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly AuthenticationDbContext _context;

        public AuthController(IConfiguration configuration, AuthenticationDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        [HttpPost("authenticate")]
        public async Task<ActionResult<TokenResponseModel>> Authenticate([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var identityUser = HttpContext.User;

            var user = await _context.Users
                .Include(u => u.RefreshToken)
                .Include(u => u.Role)
                .ThenInclude(r => r.RolePermissions)
                .FirstOrDefaultAsync(u => 
                    u.Username == model.Username &&
                    u.Password == model.Password);

            if (user == null) return Unauthorized();

            var accessToken = GenerateAccessToken(user);
            var newRefreshToken = Guid.NewGuid();

            if(user.RefreshToken == null)
            {
                user.RefreshToken = new RefreshToken
                {
                    Refresh = newRefreshToken,
                    UserId = user.Id
                };
            }
            else
            {
                user.RefreshToken.Refresh = newRefreshToken;
            }
            await _context.SaveChangesAsync();

            return Ok(new TokenResponseModel(accessToken, newRefreshToken));
        }

        [HttpPost("refresh")]
        public async Task<ActionResult<TokenResponseModel>> Refresh([FromBody] RefreshTokenModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var existingToken = await _context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.Refresh == model.RefreshToken);

            if (existingToken == null) return Unauthorized();

            var userIdClaim = HttpContext.User.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)
                ?.Value;

            if (!int.TryParse(userIdClaim, out int userId)
                || existingToken.UserId != userId)
            {
                return Unauthorized();
            }

            var user = await _context.Users
                .Include(u => u.RefreshToken)
                .Include(u => u.Role)
                .ThenInclude(r => r.RolePermissions)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null) return Unauthorized();

            var accessToken = GenerateAccessToken(user);
            var newRefreshToken = Guid.NewGuid();

            user.RefreshToken.Refresh = newRefreshToken;
            await _context.SaveChangesAsync();

            return Ok(new TokenResponseModel(accessToken, newRefreshToken));
        }

        private string GenerateAccessToken(User user)
        {
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _configuration["Issuer"] ?? throw new ArgumentException(),
                Audience = _configuration["Audience"] ?? throw new ArgumentException(),

                IssuedAt = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddMinutes(_configuration.GetValue<int>("TokenExpiryDuration")),

                Subject = new ClaimsIdentity(new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Role.Name),
                }.Concat(user.Role.RolePermissions
                    .Select(p => 
                        new Claim("Permission", ((int)p.PermissionType).ToString())))),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(_configuration["JwtTokenSecret"])), 
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var handler = new JwtSecurityTokenHandler();
            
             var jwtToken = handler.CreateJwtSecurityToken(tokenDescriptor);
             return handler.WriteToken(jwtToken);
        }
    }
}