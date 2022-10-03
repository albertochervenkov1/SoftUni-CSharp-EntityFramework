using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CarDealer.Data;
using CarDealer.DTO.Car;
using CarDealer.DTO.Customer;
using CarDealer.DTO.Part;
using CarDealer.DTO.Sale;
using CarDealer.DTO.Supplier;
using CarDealer.Models;
using Newtonsoft.Json;

namespace CarDealer
{
    public class StartUp
    {
        private static string filePath;
        public static void Main(string[] args)
        {
            Mapper.Initialize(cfg => cfg.AddProfile(typeof(CarDealerProfile)));
            var dbContext = new CarDealerContext();

            InitializeOutputFilePath("sales-discounts.json");

            string json = GetSalesWithAppliedDiscount(dbContext);
            File.WriteAllText(filePath, json);

        }

        public static string ImportSuppliers(CarDealerContext context, string inputJson)
        {
            ImportSuppliersDto[] suppliersDtos = JsonConvert.DeserializeObject<ImportSuppliersDto[]>(inputJson);

            ICollection<Supplier> suppliers = new List<Supplier>();
            foreach (var sDtos in suppliersDtos)
            {
                Supplier supplier = Mapper.Map<Supplier>(sDtos);
                suppliers.Add(supplier);
            }

            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();

            return $"Successfully imported {suppliers.Count}.";
        }

        public static string ImportParts(CarDealerContext context, string inputJson)
        {
            ImportPartsDto[] partsDtos= JsonConvert.DeserializeObject<ImportPartsDto[]>(inputJson);

            ICollection<Part> parts = new List<Part>();
            foreach (var partsDto in partsDtos)
            {
                Part part= Mapper.Map<Part>(partsDto);
                var suppliers = context.Suppliers.Select(s => s.Id);
                if (!(suppliers.Any(s=>s==part.SupplierId)))
                {
                    continue;
                }

                parts.Add(part);
            }
            context.Parts.AddRange(parts);
            context.SaveChanges();

            return $"Successfully imported {parts.Count}.";
        }

        public static string ImportCars(CarDealerContext context, string inputJson)
        {
            ImportCarsDto[] carsDtos = JsonConvert.DeserializeObject<ImportCarsDto[]>(inputJson);

            ICollection<Car> cars = new List<Car>();
            foreach (var carDto in carsDtos)
            {
                Car car = new Car()
                {
                    Make = carDto.Make,
                    Model = carDto.Model,
                    TravelledDistance = carDto.TravelledDistance
                };

                foreach (int partId in carDto.PartsId.Distinct())
                {
                    car.PartCars.Add(new PartCar()
                    {
                        Car = car,
                        PartId = partId
                    });
                }

                cars.Add(car);
            }

            context.Cars.AddRange(cars);
            context.SaveChanges();

            return $"Successfully imported {cars.Count}.";
        }

        public static string ImportCustomers(CarDealerContext context, string inputJson)
        {
            ImportCustomersDto[] customersDtos = JsonConvert.DeserializeObject<ImportCustomersDto[]>(inputJson);

            ICollection<Customer> customers = new List<Customer>();
            foreach (var dto in customersDtos)
            {
                Customer customer = new Customer()
                {
                    Name = dto.Name,
                    BirthDate = dto.BirthDate,
                    IsYoungDriver = dto.IsYoungDriver
                };
                customers.Add(customer);
            }

            context.Customers.AddRange(customers);
            context.SaveChanges();

            return $"Successfully imported {customers.Count}.";
        }

        public static string ImportSales(CarDealerContext context, string inputJson)
        {
            ImportSalesDto[] salesDtos= JsonConvert.DeserializeObject<ImportSalesDto[]>(inputJson);

            ICollection<Sale> sales= new List<Sale>();
            foreach (var dto in salesDtos)
            {
                Sale sale = Mapper.Map<Sale>(dto);
                sales.Add(sale);
            }

            context.Sales.AddRange(sales);
            context.SaveChanges();

            return $"Successfully imported {sales.Count}.";
        }

        public static string GetOrderedCustomers(CarDealerContext context)
        {
            var customers = context.Customers
                
                .OrderBy(x => x.BirthDate)
                .ThenBy(x => x.IsYoungDriver)
                .ProjectTo<ExportOrderedCustomersDto>()
                .ToArray();

            var settings = new JsonSerializerSettings()
            {
                DateFormatString = "dd/MM/yyyy"
            };
            string json = JsonConvert.SerializeObject(customers, Formatting.Indented,settings);
            return json;
        }

        public static string GetCarsFromMakeToyota(CarDealerContext context)
        {
            var cars = context.Cars
                .Where(c => c.Make == "Toyota")
                .OrderBy(c => c.Model)
                .ThenByDescending(c => c.TravelledDistance)
                .ProjectTo<ExportToyotaCarsDto>()
                .ToList();

            string json = JsonConvert.SerializeObject(cars, Formatting.Indented);

            return json;
        }

        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var suppliers = context.Suppliers
                .Where(s => s.IsImporter == false)
                .ProjectTo<ExportSuppliersNotAbroadDto>()
                .ToList();
            var json =JsonConvert.SerializeObject(suppliers, Formatting.Indented);
            return json;
        }

        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var cars = context.Cars
                .Select(c => new
                {
                    car = new
                    {
                        Make = c.Make,
                        Model = c.Model,
                        TravelledDistance = c.TravelledDistance
                    },
                    parts = c.PartCars.Select(pc => new
                    {
                        Name = pc.Part.Name,
                        Price = $"{pc.Part.Price:F2}"
                    }).ToArray(),
                }).ToArray();

            var jsonFile = JsonConvert.SerializeObject(cars, Formatting.Indented);

            return jsonFile;
        }

        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            var customers = context.Customers
                .Where(c => c.Sales.Any())
                .Select(c => new
                {
                    fullName = c.Name,
                    boughtCars = c.Sales.Count(),
                    spentMoney = c.Sales.Sum(s => s.Car.PartCars.Sum(pc => pc.Part.Price))
                })
                .OrderByDescending(c => c.spentMoney)
                .ThenByDescending(c => c.boughtCars)
                .ToArray();

            var jsonFile = JsonConvert.SerializeObject(customers, Formatting.Indented);

            return jsonFile;
        }

        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            var sales = context.Sales
                .Take(10)
                .Select(s => new
                {
                    car = new
                    {
                        Make = s.Car.Make,
                        Model = s.Car.Model,
                        TravelledDistance = s.Car.TravelledDistance
                    },
                    customerName = s.Customer.Name,
                    Discount = $"{s.Discount:F2}",
                    price = $"{s.Car.PartCars.Sum(pc => pc.Part.Price):F2}",
                    priceWithDiscount = $"{s.Car.PartCars.Sum(pc => pc.Part.Price) * (1 - (s.Discount / 100)):F2}",
                })
                .ToArray();

            var jsonFile = JsonConvert.SerializeObject(sales, Formatting.Indented);

            return jsonFile;
        }

        private static void InitializeOutputFilePath(string fileName)
        {
            filePath = Path.Combine(Directory.GetCurrentDirectory(), "../../../Results/", fileName);
        }
    }
}