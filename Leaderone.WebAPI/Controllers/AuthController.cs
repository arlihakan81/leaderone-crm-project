using Leaderone.Application.Interfaces;
using Leaderone.Application.Requests;
using Leaderone.Domain.Entities;
using Leaderone.Persistence.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Leaderone.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IConfiguration config, IAppUserRepository appUserRepo) : ControllerBase
    {
        private readonly IConfiguration _config = config;
        private readonly IAppUserRepository _appUserRepo = appUserRepo;

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequest request)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _appUserRepo.GetUserByEmailAsync(request.Email);
            if(user is null)
            {
                return Unauthorized("Invalid email or password.");
            }

            if(new PasswordHasher<AppUser>().VerifyHashedPassword(user, user.PasswordHash, request.Password) == PasswordVerificationResult.Failed)
            {
                return Unauthorized("Invalid email or password.");
            }

            return Ok(new { Token = GenerateJwtToken(user) });

        }

        private string GenerateJwtToken(AppUser user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("TenantId", user.TenantId.ToString())
            };

            var secretKey = _config["Jwt:SecretKey"];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody]RegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingUser = await _appUserRepo.GetUserByEmailAsync(request.Email);
            if (existingUser is not null)
            {
                return Conflict("Email is already in use.");
            }

             
            var tenant = new Tenant();
            using (var context = new LeaderoneDbContext())
            {
                var name = request.Email.Split('@')[1];
                if(!context.Tenants.Any(t => t.Name == name))
                {
                    tenant.Name = name.Split('.')[0];
                    tenant.Domain = request.Email.Split('@')[1];
                    context.Tenants.Add(tenant);
                    await context.SaveChangesAsync();
                }
            };

            var user = new AppUser
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PasswordHash = new PasswordHasher<AppUser>().HashPassword(null!, request.Password),
                TenantId = tenant.Id
            };

            user.PasswordHash = new PasswordHasher<AppUser>().HashPassword(user, request.Password);

            await _appUserRepo.AddAsync(user);

            return Ok("Your register operation succeed");
        }


    }
}
