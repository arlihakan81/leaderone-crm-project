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

            return Ok(GenerateJwtToken(user));

        }

        private string GenerateJwtToken(AppUser user)
        {
            var claims = new[]
            {
                new Claim("avatar", user.Avatar ?? string.Empty),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.RoleInTenant.ToString()),
                new Claim("TenantId", user.TenantId.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:SecretKey"]!));
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

            var domain = request.Email.Split('@')[1];
            var tenant = new Tenant();
            using (var context = new LeaderoneDbContext())
            {
                if(!context.Tenants.Any(t => t.Domain == domain))
                {
                    tenant.Name = domain.Split('.')[0];
                    tenant.Domain = request.Email.Split('@')[1];
                    context.Tenants.Add(tenant);
                    await context.SaveChangesAsync();
                }
                tenant = context.Tenants.FirstOrDefault(t => t.Domain == domain);

                var user = new AppUser
                {
                    Id = Guid.NewGuid(),
                    Avatar = "avatar.webp",
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                    PasswordHash = new PasswordHasher<AppUser>().HashPassword(null!, request.Password),
                    RoleInTenant = Domain.Enums.Enumeration.TenantRole.Admin,
                    TenantId = tenant!.Id
                };

                if(!await _appUserRepo.IsExistsAdminAsync(tenant.Id))
                { 
                    await _appUserRepo.AddAsync(user);                    
                }
            };

            return Ok("Your register operation succeed");
        }


    }
}
