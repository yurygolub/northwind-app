using AutoMapper;
using Northwind.Services.Employees;
using Northwind.Services.EntityFrameworkCore.Entities;
using Northwind.Services.Products;

namespace Northwind.Services.EntityFrameworkCore.MappingProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            this.CreateMap<Employee, EmployeeEntity>().ReverseMap();
            this.CreateMap<Product, ProductEntity>().ReverseMap();
            this.CreateMap<ProductCategory, CategoryEntity>().ReverseMap();
        }
    }
}
