using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using TemplateApi.Core.Models;

namespace TemplateApi.Common.Extensions
{
    public static class DatabaseExtensions
    {
        public static void AddDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            // Connection String / DbContext
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<AppDbContext>(
                options => options.UseSqlServer(connectionString)  
            );
            // Dapper for SQL Server (SqlConnection)
            services.AddScoped<IDbConnection>(
                sp => new SqlConnection(connectionString)  
            );
        }
    }
}