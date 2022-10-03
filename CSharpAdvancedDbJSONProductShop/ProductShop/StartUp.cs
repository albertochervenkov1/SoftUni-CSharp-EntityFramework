using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ProductShop.Data;
using ProductShop.DTOs.Category;
using ProductShop.DTOs.CategoryProduct;
using ProductShop.DTOs.Product;
using ProductShop.DTOs.User;
using ProductShop.Models;

namespace ProductShop
{
   
    public class StartUp
    {
        private static string filePath;
        public static void Main(string[] args)
        {
            ProductShopContext dbcontext = new ProductShopContext();
            
            Mapper.Initialize(cfg=>cfg.AddProfile(typeof(ProductShopProfile)));
            
            InitializeOutputFilePath("users-and-products.json");

            string json = GetUsersWithProducts(dbcontext);
            File.WriteAllText(filePath,json);
            
           
        }

        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            ImportUsersDto[] usersDtos = JsonConvert.DeserializeObject<ImportUsersDto[]>(inputJson);

            ICollection<User> users = new List<User>();
            foreach (ImportUsersDto uDto in usersDtos)
            {
                User user = Mapper.Map<User>(uDto);
                users.Add(user);
            }
            context.Users.AddRange(users);
            context.SaveChanges();
            
            return $"Successfully imported {users.Count}";
        }

        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
            ImportProductDto[] productDtos=JsonConvert.DeserializeObject<ImportProductDto[]>(inputJson);

            ICollection<Product> products= new List<Product>();
            foreach (var uDto in productDtos)
            {
                Product product=Mapper.Map<Product>(uDto);

                products.Add(product);
            }
            context.Products.AddRange(products);
            context.SaveChanges();

            return $"Successfully imported {products.Count}";
        }

        public static string ImportCategories(ProductShopContext context, string inputJson)
        {
            ImportCategoryDto[] categoryDtos = JsonConvert.DeserializeObject<ImportCategoryDto[]>(inputJson);
            ICollection<Category> categories= new List<Category>();
            foreach (var uDto in categoryDtos)
            {
                Category category = Mapper.Map<Category>(uDto);
                if (category.Name==null)
                {
                    continue;
                    
                }
                categories.Add(category);
            }
            context.Categories.AddRange(categories);
            context.SaveChanges();
            return $"Successfully imported {categories.Count}";
        }

        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {
            ImportCategoriesProductsDto[] categoriesProductsDtos =
                JsonConvert.DeserializeObject<ImportCategoriesProductsDto[]>(inputJson);

            ICollection<CategoryProduct> categoriesProducts= new List<CategoryProduct>();
            foreach (var uDto in categoriesProductsDtos)
            {
                CategoryProduct categoryProduct = Mapper.Map<CategoryProduct>(uDto);
                categoriesProducts.Add(categoryProduct);
            }
            context.CategoryProducts.AddRange(categoriesProducts);
            context.SaveChanges();
            return $"Successfully imported {categoriesProducts.Count}";
        }

        public static string GetProductsInRange(ProductShopContext context)
        {
            var products = context.Products
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .OrderBy(p=>p.Price)
                .ProjectTo<ExportProductsInRangeDto>()
                .ToList();

            string json = JsonConvert.SerializeObject(products,Formatting.Indented);
            return json;
        }

        public static string GetSoldProductss(ProductShopContext context)
        {
            var users = context.Users
                .Where(u => u.ProductsSold.Any(p=>p.BuyerId.HasValue))
                .Select(u => new
                {
                    firstName = u.FirstName,
                    lastName = u.LastName,
                    soldProducts = u.ProductsSold
                        .Where(p=>p.BuyerId.HasValue)
                        .Select(ps => new
                        {
                            
                            name = ps.Name,
                            price = ps.Price,
                            buyerFirstName = ps.Buyer.FirstName,
                            buyerLastName = ps.Buyer.LastName,
                        })
                        .ToList()

                })
                .OrderBy(u => u.lastName)
                .ThenBy(u => u.firstName)
                .ToList();
            string json = JsonConvert.SerializeObject(users, Formatting.Indented);
            return json;
        }
        public static string GetSoldProducts(ProductShopContext context)
        {
            ExportUsersWithSoldProductsDto[] users = context.Users
                .Where(u => u.ProductsSold.Any(p => p.BuyerId.HasValue))
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .ProjectTo<ExportUsersWithSoldProductsDto>()
                .ToArray();

            string json = JsonConvert.SerializeObject(users, Formatting.Indented);
            return json;
        }

        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var categories = context.Categories
                .ProjectTo<ExportCategoryByProductsCountDto>()
                .OrderByDescending(c => c.ProductsCount)
                .ToArray();
            string json= JsonConvert.SerializeObject(categories, Formatting.Indented);
            return json;
        }

       
        public static string GetUsersWithProducts(ProductShopContext context)
        {
            var users = context.Users
                .Include(x => x.ProductsSold)
                .ToList()
                .Where(u => u.ProductsSold.Any(p => p.BuyerId != null))
                .Select(u => new
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Age = u.Age,
                    SoldProducts = new
                    {
                        Count = u.ProductsSold
                            .Where(p => p.BuyerId != null).Count(),
                        Products = u.ProductsSold
                            .Where(p => p.BuyerId != null)
                            .Select(p => new
                            {
                                Name = p.Name,
                                Price = p.Price
                            })
                    }
                })
                .OrderByDescending(x => x.SoldProducts.Count)
                .ToList();
            var resultObject = new
            {
                usersCount = users.Count,
                users = users
            };
            DefaultContractResolver contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };
            var jsonSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = contractResolver
            };
            var jsonResult = JsonConvert.SerializeObject(resultObject, jsonSettings);
            return jsonResult;
        }
        private static void InitializeOutputFilePath(string fileName)
        {
            filePath = Path.Combine(Directory.GetCurrentDirectory(), "../../../Results/", fileName);
        }
    }
}