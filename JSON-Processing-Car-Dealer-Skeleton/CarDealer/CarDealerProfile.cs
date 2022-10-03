using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;
using AutoMapper;
using CarDealer.DTO.Car;
using CarDealer.DTO.Customer;
using CarDealer.DTO.Part;
using CarDealer.DTO.Sale;
using CarDealer.DTO.Supplier;
using CarDealer.Models;

namespace CarDealer
{
    public class CarDealerProfile : Profile
    {
        public CarDealerProfile()
        {
            this.CreateMap<ImportSuppliersDto, Supplier>();
            this.CreateMap<ImportPartsDto, Part>();
            this.CreateMap<ImportCarsDto, Car>();
            this.CreateMap<ImportCustomersDto, Customer>();
            this.CreateMap<ImportSalesDto, Sale>();

            this.CreateMap<Customer, ExportOrderedCustomersDto>();
            this.CreateMap<Car, ExportToyotaCarsDto>();
            this.CreateMap<Supplier, ExportSuppliersNotAbroadDto>()
                .ForMember(d => d.PartsCount, mo =>
                    mo.MapFrom(s => s.Parts.Count));


        }
    }
}
