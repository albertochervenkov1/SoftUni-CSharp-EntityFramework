using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using AutoMapper;
using ProductShop.DTOs.Category;
using ProductShop.DTOs.CategoryProduct;
using ProductShop.DTOs.Product;
using ProductShop.DTOs.User;
using ProductShop.Models;

namespace ProductShop
{
    public class ProductShopProfile : Profile
    {
        public ProductShopProfile()
        {
            this.CreateMap<ImportUsersDto, User>();
            this.CreateMap<ImportProductDto, Product>();
            this.CreateMap<ImportCategoryDto, Category>();
            this.CreateMap<ImportCategoriesProductsDto, CategoryProduct>();

            this.CreateMap<Product, ExportProductsInRangeDto>()
                .ForMember(d => d.SellerFullName,
                    mo => mo.MapFrom(s => $"{s.Seller.FirstName} {s.Seller.LastName}"));
            this.CreateMap<Product, ExportUserSoldProductsDto>()
                .ForMember(d => d.BuyerFirstName,
                    mo => mo.MapFrom(s => s.Buyer.FirstName))
                .ForMember(d => d.BuyerLastName, 
                    mo => mo.MapFrom(s => s.Buyer.LastName));

            this.CreateMap<User, ExportUsersWithSoldProductsDto>()
                .ForMember(d => d.SoldProducts,
                    mo => mo.MapFrom(s => s.ProductsSold
                        .Where(p=>p.BuyerId.HasValue)));

            this.CreateMap<Product, ExportSoldProductsShortInfoDto>();

            this.CreateMap<User, ExportSoldProductsFullInfoDto>()
                .ForMember(d => d.SoldProducts, mo =>
                    mo.MapFrom(s => s.ProductsSold.Where(p => p.BuyerId.HasValue)));
            this.CreateMap<User, ExportUsersWithFullProductInfoDto>()
                .ForMember(d => d.SoldProducts, mo =>
                    mo.MapFrom(s => s));

        }
    }
}
