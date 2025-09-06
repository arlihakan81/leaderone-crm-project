using Leaderone.Application.Interfaces;
using Leaderone.Application.Requests;
using Leaderone.Domain.Entities;
using Leaderone.Domain.Enums;
using Leaderone.Persistence.Context;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;

namespace Leaderone.WebUI.Controllers
{
    public class AuthController(HttpClient apiClient, IAppUserRepository repository) : Controller
    {
        private readonly IAppUserRepository repository = repository;
        private readonly HttpClient _apiClient = apiClient;

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Please fill in all required fields.");
                return View(request);
            }

            var user = await repository.GetUserByEmailAsync(request.Email);
            if (user == null)
            {
                ModelState.AddModelError(request.Password, "Invalid email or password.");
                return View(request);
            }

            // If the user is found and password matches, create claims and sign in
            if (new PasswordHasher<AppUser>().VerifyHashedPassword(user, user.PasswordHash, request.Password) == PasswordVerificationResult.Failed)
            {
                ModelState.AddModelError(request.Password, "Invalid email or password.");
                return View(request);
            }

            var response = await _apiClient.PostAsJsonAsync<LoginRequest>($"{_apiClient.BaseAddress}/login", request);
            response.EnsureSuccessStatusCode();
            var token = await response.Content.ReadFromJsonAsync<string>();
            HttpContext.Response.Cookies.Append("jwt_token", token!, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddHours(1) // Set expiration time as needed
            });
            var claims = new List<Claim>
            {
                new ("TenantId", user.TenantId.ToString()),
                new ("avatar", user.Avatar ?? string.Empty),
                new (ClaimTypes.NameIdentifier, user.Id.ToString()),
                new (ClaimTypes.Name, user.FullName),
                new (ClaimTypes.Email, user.Email),
                new (ClaimTypes.Role, user.RoleInTenant.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1) // Set expiration time as needed
            });

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password) || string.IsNullOrEmpty(request.FirstName) || string.IsNullOrEmpty(request.LastName))
            {
                ModelState.AddModelError(request.Email, "Invalid registration request.");
                return View();
            }

            var existingUser = await repository.GetUserByEmailAsync(request.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError("Email", "User with this email already exists.");
                return View();
            }

            var tenant = new Tenant();
            using (var context = new LeaderoneDbContext())
            {
                var name = request.Email.Split('@')[1];
                if (!context.Tenants.Any(t => t.Name == name))
                {
                    tenant.Name = name.Split('.')[0];
                    tenant.Domain = request.Email.Split('@')[1];
                    context.Tenants.Add(tenant);
                    await context.SaveChangesAsync();
                    tenant = context.Tenants.FirstOrDefault(t => t.Name == name);
                }
            };

            var user = new AppUser
            {
                Id = Guid.NewGuid(),
                Avatar = "avatar.webp",
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                PasswordHash = new PasswordHasher<AppUser>().HashPassword(null!, request.Password),
                RoleInTenant = Enumeration.TenantRole.Admin,
                TenantId = tenant.Id
            };

            if (await repository.IsExistsAdminAsync(tenant.Id))
            {
                user.RoleInTenant = Enumeration.TenantRole.Employee;
                await repository.AddAsync(user);
            }
            return RedirectToAction("Login");
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout() 
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

    }
}
