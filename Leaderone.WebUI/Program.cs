using Leaderone.Application.Interfaces;
using Leaderone.Application.Repositories;
using Leaderone.Persistence.Context;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped(t => new HttpClient { BaseAddress = new Uri("https://localhost:7085/api") });
builder.Services.AddDbContext<LeaderoneDbContext>();
builder.Services.AddScoped<IAppUserRepository, AppUserRepository>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.AccessDeniedPath = "/Auth/Forbidden";
        options.LoginPath = "/Auth/Login";
        options.LogoutPath = "/Auth/Logout";
        options.ExpireTimeSpan = TimeSpan.FromHours(1); // Set expiration time as needed
        options.SlidingExpiration = true; // Optional: Enable sliding expiration
        options.Cookie.HttpOnly = true; // Set cookie to HttpOnly for security
        options.Cookie.SecurePolicy = Microsoft.AspNetCore.Http.CookieSecurePolicy.Always; // Use secure cookies
        options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Strict; // Set SameSite policy
        options.Cookie.HttpOnly = true; // Prevent JavaScript access to the cookie
    });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
