using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingService.Service.Interface;

namespace BookingService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastruction(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = (configuration["UseEnv"] ?? "0") == "0" ? configuration["Database"] : $"Server={Environment.GetEnvironmentVariable("MSSQL_URL")},{Environment.GetEnvironmentVariable("MSSQL_PORT")};Database={Environment.GetEnvironmentVariable("MSSQL_DB")};User Id={Environment.GetEnvironmentVariable("MSSQL_USER")};Password={Environment.GetEnvironmentVariable("MSSQL_PASS")};Persist Security Info=False;MultipleActiveResultSets =False;Encrypt=True;TrustServerCertificate=False";
            services.AddDbContext<BookingDbContext>(opt => opt.UseSqlServer(connectionString, b => b.MigrationsAssembly("BookingService.Infrastructure")));

            services.AddScoped<IServiceManager, ServiceManager>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<ICurrentUserService, CurrentUserService>();
            return services;
        }
    }
}
