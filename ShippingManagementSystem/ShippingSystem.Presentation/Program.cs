using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ShippingSystem.Domain.Entities;
using ShippingSystem.Domain.Interfaces;
using ShippingSystem.Domain.IUnitWorks;
using ShippingSystem.Infrastructure.Auth;
using ShippingSystem.Infrastructure.Data;
using ShippingSystem.Infrastructure.Repositories;
using ShippingSystem.Infrastructure.UnitWorks;
using ShippingSystem.Presentation.Extensions;
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
            })
            .AddEntityFrameworkStores<ShippingDbContext>()
            .AddDefaultTokenProviders();
            builder.Services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, ApplicationUserClaimsPrincipalFactory>();
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            //builder.Services.AddScoped<IGovernorateRepository, GovernorateRepository>();
            //builder.Services.AddScoped<ICityRepository, CityRepository>();
            //builder.Services.AddScoped<IWeightSettingsRepository, WeightSettingsRepository>();
            //builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            //builder.Services.AddScoped<ICourierRepository, CourierRepository>();
            //builder.Services.AddScoped<IMerchantRepository, MerchantRepository>();
            //builder.Services.AddScoped<IOrderRepository, OrderRepository>();
            //builder.Services.AddScoped<IOrderItemsRepository, OrderItemsRepository>();

            //Add Permissions
            builder.Services.AddPermissionPolicies();
 
            //Auto Mapper
            builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
            //Sessions
            // Add services to the container
            builder.Services.AddControllersWithViews();

            //Add Session services
            builder.Services.AddDistributedMemoryCache(); 
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            //Add Redis Server
            builder.Services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = "localhost:6379";
                options.InstanceName = "ShippingManagementSystem_";
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
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();  //Use session

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
