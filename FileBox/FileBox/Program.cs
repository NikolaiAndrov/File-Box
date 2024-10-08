namespace FileBox
{
    using FileBox.Data;
    using FileBox.Services;
    using FileBox.Services.Interfaces;
    using Microsoft.AspNetCore.Http.Features;
    using Microsoft.EntityFrameworkCore;

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            string connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            // Add services to the container.
            builder.Services.AddDbContext<FileBoxDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            // Configure form options to allow large uploads
            builder.Services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = long.MaxValue; // Maximum possible size
            });

            builder.WebHost.ConfigureKestrel(options =>
            {
                options.Limits.MaxRequestBodySize = 10L * 1024 * 1024 * 1024; // 10 GB
            });

            builder.Services.AddControllersWithViews();
            builder.Services.AddScoped<IFileService, FileService>();

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

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}