using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ShippingSystem.Application.Interfaces;
using ShippingSystem.Application.Services;
using ShippingSystem.Domain.Entities;
using ShippingSystem.Domain.Interfaces;
using ShippingSystem.Infrastructure.Data;
using ShippingSystem.Infrastructure.Repositories;
using System.Reflection;

namespace ShippingSystem.Presentation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            //Connection String
            builder.Services.AddDbContext<ShippingDbContext>(Options =>
            
                Options.UseSqlServer(builder.Configuration.GetConnectionString("ShippingSystemCS")));
            //Identity User & Identity Role
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(Options => {
                Options.Password.RequiredLength = 8;
                Options.Password.RequireNonAlphanumeric = false;
                Options.Password.RequireDigit = true;
            }).AddEntityFrameworkStores<ShippingDbContext>();

            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddScoped(typeof(IGenericService<>), typeof(GenericService<>));
            builder.Services.AddScoped<IGovernorateService, GovernorateService>();
            builder.Services.AddScoped<IGovernorateRepository, GovernorateRepository>();

            //Auto Mapper
            builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
