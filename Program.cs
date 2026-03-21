using KhoaNoiVuCNTT.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

namespace KhoaNoiVuCNTT
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<ThongTinNoiBoContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("MyStoreCtx")
                ));

            builder.Services.AddControllersWithViews(options =>
            {
                var policy = new Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();

                options.Filters.Add(new Microsoft.AspNetCore.Mvc.Authorization.AuthorizeFilter(policy));
            });
            // Authentication
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LogoutPath = "/TaiKhoan/Logout";
                    options.AccessDeniedPath = "/TaiKhoan/AccessDenied";
                });

            builder.Services.AddAuthorization();

            var app = builder.Build();

            // Middleware
            if (!app.Environment.IsDevelopment())   
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            // Route cho Area
            app.MapControllerRoute(
                name: "areas",
                pattern: "{area:exists}/{controller=Trangchu}/{action=Index}/{id?}");
            // Route mặc định
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}