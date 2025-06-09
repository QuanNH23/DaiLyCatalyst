using HealMe.Models;
using HealMe.Services;
using Microsoft.EntityFrameworkCore;

namespace HealMe
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<HealMeDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddScoped<DAO.ITaskDAO, DAO.TaskDAO>();
            builder.Services.AddScoped<ITaskService, TaskService>();
            builder.Services.AddScoped<DAO.IUserDAO, DAO.UserDAO>();
            builder.Services.AddScoped<Services.IUserService, Services.UserService>();
            builder.Services.AddScoped<IFeedbackService, FeedbackService>();
            builder.Services.AddScoped<IFileService, FileService>();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromHours(6); // Thời gian hết hạn phiên
                options.Cookie.HttpOnly = true; // Chỉ cho phép truy cập cookie từ máy chủ
                options.Cookie.IsEssential = true; // Cookie cần thiết cho ứng dụng
            });
            // CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
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
            app.UseSession();
            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
