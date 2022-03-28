using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Northwind.Services;
using Northwind.Services.Employees;
using Northwind.Services.Products;
using Northwind.Services.SqlServer;
using DataAccess = Northwind.Services.DataAccess;

#pragma warning disable SA1600

namespace NorthwindApiApp
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddSqlServices(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                .AddTransient<IProductManagementService, DataAccess.Products.ProductManagementService>()
                .AddTransient<IProductCategoryManagementService, DataAccess.Products.ProductCategoryManagementService>()
                .AddTransient<IProductCategoryPicturesService, DataAccess.Products.ProductCategoryPicturesService>()
                .AddTransient<IEmployeeManagementService, DataAccess.Employees.EmployeeManagementService>()
                .AddTransient<IEmployeePicturesService, DataAccess.Employees.EmployeePicturesService>()
                .AddScoped(s => new SqlConnection(configuration.GetConnectionString("SqlConnection")))
                .AddTransient<NorthwindDataAccessFactory, SqlServerDataAccessFactory>();
        }
    }
}
