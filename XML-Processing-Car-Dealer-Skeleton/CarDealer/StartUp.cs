using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using CarDealer.Data;
using CarDealer.DTO.ExportDto;
using CarDealer.DTO.ImportDto;


using CarDealer.Models;

namespace CarDealer
{
    public class StartUp
    {
        
        public static void Main(string[] args)
        {
            CarDealerContext dbContext=new CarDealerContext();

            //string inputXml = File.ReadAllText("../../../Datasets/sales.xml");

            

            string xml = GetSalesWithAppliedDiscount(dbContext);
            Console.WriteLine(xml);
            
        }

        //Import
        public static string ImportSuppliers(CarDealerContext context, string inputXml)
        {
            XmlRootAttribute xmlRoot = new XmlRootAttribute("Suppliers");
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportSupplierDto[]),xmlRoot);

           using StringReader stringReader=new StringReader(inputXml);
           ImportSupplierDto[] dtos = (ImportSupplierDto[])xmlSerializer.Deserialize(stringReader);

           ICollection<Supplier> suppliers = new HashSet<Supplier>();

           foreach (var dto in dtos)
           {
               Supplier supplier=new Supplier()
               {
                   Name=dto.Name,
                   IsImporter = bool.Parse(dto.IsImporter)
               };
               suppliers.Add(supplier);
           }
           context.Suppliers.AddRange(suppliers);
           context.SaveChanges();
           return $"Successfully imported {suppliers.Count}";
        }

        public static string ImportParts(CarDealerContext context, string inputXml)
        {
            XmlRootAttribute xmlRoot = new XmlRootAttribute("Parts");
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportPartsDto[]), xmlRoot);

            using StringReader stringReader=new StringReader(inputXml);
            ImportPartsDto[] dtos = (ImportPartsDto[])xmlSerializer.Deserialize(stringReader);

            ICollection<Part> parts = new HashSet<Part>();

            foreach (var dto in dtos)
            {
                Supplier supplier = context.Suppliers.Find(dto.SupplierId);

                if (supplier==null)
                {
                    continue;
                }

                Part part = new Part()
                {
                    Name = dto.Name,
                    Price = decimal.Parse(dto.Price),
                    Quantity = dto.Quantity,
                    SupplierId = dto.SupplierId
                };
                parts.Add(part);
            }
            context.Parts.AddRange(parts);
            context.SaveChanges();

            return $"Successfully imported {parts.Count}";
        }

        public static string ImportCars(CarDealerContext context, string inputXml)
        {
            XmlSerializer xmlSerializer = GenerateXmlSerializer("Cars", typeof(ImportCarDto[]));

            using StringReader stringReader=new StringReader(inputXml);
            ImportCarDto[] carDtos = (ImportCarDto[])xmlSerializer.Deserialize(stringReader);

            ICollection<Car> cars = new HashSet<Car>();
            
            foreach (var carDto in carDtos)
            {
                Car c = new Car()
                {
                    Make = carDto.Make,
                    Model = carDto.Model,
                    TravelledDistance = carDto.TravelledDistance
                };
               ICollection<PartCar> carParts=new HashSet<PartCar>();
               foreach (int partId in carDto.Parts.Select(p=>p.Id).Distinct())
               {
                   Part part = context.Parts.Find(partId);
                   
                   if (part == null)
                   {
                       continue;
                   }

                   PartCar partCar = new PartCar()
                   {
                       Car = c,
                       Part = part
                   };
                   carParts.Add(partCar);

               }
               c.PartCars=carParts;
               cars.Add(c);
            }

            context.Cars.AddRange(cars);
            context.SaveChanges();
            return $"Successfully imported {cars.Count}";
        }

        public static string ImportCustomers(CarDealerContext context, string inputXml)
        {
            XmlSerializer xmlSerializer = GenerateXmlSerializer("Customers", typeof(ImportCustomerDto[]));

            using StringReader stringReader=new StringReader(inputXml);
            ImportCustomerDto[] customerDtos=(ImportCustomerDto[])xmlSerializer.Deserialize(stringReader);

            ICollection<Customer> customers = new HashSet<Customer>();
            foreach (var dto in customerDtos)
            {
                Customer customer = new Customer()
                {
                    Name = dto.Name,
                    BirthDate = DateTime.Parse(dto.BirthDate,CultureInfo.CurrentCulture),
                    IsYoungDriver = bool.Parse(dto.IsYoungDriver)
                };
                customers.Add(customer);
            }
            context.Customers.AddRange(customers);
            context.SaveChanges();
            return $"Successfully imported {customers.Count}";
        }

        public static string ImportSales(CarDealerContext context, string inputXml)
        {
            XmlSerializer xmlSerializer = GenerateXmlSerializer("Sales", typeof(ImportSaleDto[]));
            using StringReader inputReader=new StringReader(inputXml);

            ImportSaleDto[] dtos = (ImportSaleDto[])xmlSerializer.Deserialize(inputReader);
            ICollection<Sale> sales = new HashSet<Sale>();

            foreach (var dto in dtos)
            {
                Car car = context.Cars.Find(dto.CarId);
                if (car==null)
                {
                    continue;
                    
                }

                Sale sale = new Sale()
                {
                    Car = car,
                    CustomerId = dto.CustomerId,
                    Discount = dto.Discount
                };
                sales.Add(sale);
            }
            context.Sales.AddRange(sales);
            context.SaveChanges();

            return $"Successfully imported {sales.Count}";
        }

        //Export
        

        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            StringBuilder sb = new StringBuilder();
            using StringWriter sw = new StringWriter(sb);

            XmlSerializer xmlSerializer = GenerateXmlSerializer("sales", typeof(ExportSalesWithDiscountDto[]));
            XmlSerializerNamespaces namespaces=new XmlSerializerNamespaces();
            namespaces.Add(String.Empty, String.Empty);

            ExportSalesWithDiscountDto[] dtos = context.Sales
                .Select(s => new ExportSalesWithDiscountDto()
                {
                    Car = new ExportSalesCarDto()
                    {
                        Make = s.Car.Make,
                        Model = s.Car.Model,
                        TravelledDistance = s.Car.TravelledDistance.ToString()
                    },
                    Discount = s.Discount.ToString(CultureInfo.InvariantCulture),
                    CustomerName = s.Customer.Name,
                    Price = s.Car.PartCars.Sum(pc => pc.Part.Price).ToString(CultureInfo.InvariantCulture),
                    PriceWithDiscount = (s.Car.PartCars.Sum(pc => pc.Part.Price) -
                                         s.Car.PartCars.Sum(pc => pc.Part.Price)
                                         * s.Discount/100).ToString(CultureInfo.InvariantCulture)

                })
                .ToArray();
            xmlSerializer.Serialize(sw,dtos,namespaces);

            return sb.ToString().TrimEnd();
        }


        private static XmlSerializer GenerateXmlSerializer(string rootName, Type dtoType)
        {
            XmlRootAttribute xmlRoot = new XmlRootAttribute(rootName);
            XmlSerializer xmlSerializer = new XmlSerializer(dtoType, xmlRoot);

            return xmlSerializer;
        }
        
    }
}