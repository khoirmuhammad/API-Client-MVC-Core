using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using VoxTestApplication.Data;
using VoxTestApplication.Extensions;
using VoxTestApplication.Helpers;
using VoxTestApplication.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options =>
        {
            options.LoginPath = "/Auth";
            // Specify the name of the auth cookie.
            // ASP.NET picks a dumb name by default. "AspNetCore.Cookies"
            options.Cookie.Name = "my_app_auth_cookie";
        });

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped(typeof(IHttpClientHelper<>), typeof(HttpClientHelper<>));

builder.Services.AddScoped<IApplicationLogRepository, ApplicationLogRepository>();

builder.Services.AddAutoMapper(typeof(Program));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseMiddleware<GlobalExceptionMiddleware>(); // custom global exception

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Index}/{id?}");

app.Run();
