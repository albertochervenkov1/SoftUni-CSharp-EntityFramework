
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Artillery.Data.Models.Enums;
using Artillery.DataProcessor.ExportDto;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Artillery.DataProcessor
{
    using Artillery.Data;
    using System;

    public class Serializer
    {
        public static string ExportShells(ArtilleryContext context, double shellWeight)
        {
            var shells = context.Shells
                .Where(s => s.ShellWeight > shellWeight)
                .Select(s => new
                {
                    ShellWeight = s.ShellWeight,
                    Caliber = s.Caliber,
                    Guns = s.Guns
                        .Where(g => g.GunType == GunType.AntiAircraftGun)
                        .Select(g=>new
                        {
                            GunType=g.GunType.ToString(),
                            GunWeight=g.GunWeight,
                            BarrelLength=g.BarrelLength,
                            Range=g.Range>3000? "Long-range": "Regular range"
                        })
                        .OrderByDescending(g=>g.GunWeight)
                        .ToArray()
                })
                .OrderBy(s=>s.ShellWeight)
                .ToArray();

            string json = JsonConvert.SerializeObject(shells, Formatting.Indented);

            return json;
        }

        public static string ExportGuns(ArtilleryContext context, string manufacturer)
        {
            StringBuilder sb=new StringBuilder();

            XmlRootAttribute xmlRoot = new XmlRootAttribute("Guns");
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ExportGunDto[]), xmlRoot);

            using StringWriter sw = new StringWriter(sb);

            ExportGunDto[] guns = context.Guns
                .ToArray()
                .Where(g => g.Manufacturer.ManufacturerName == manufacturer)
                .Select(g => new ExportGunDto()
                {
                    Manufacturer = g.Manufacturer.ManufacturerName,
                    GunType = g.GunType.ToString(),
                    GunWeight = g.GunWeight.ToString(),
                    BarrelLength = g.BarrelLength.ToString(),
                    Range = g.Range.ToString(),
                    Countries = g.CountriesGuns
                        .Where(gc => gc.Country.ArmySize > 4500000)
                        .Select(gc => new ExportGunCountryDto()
                        {
                            Country = gc.Country.CountryName,
                            ArmySize = gc.Country.ArmySize.ToString()
                        })
                        .OrderBy(gc => gc.ArmySize)
                        .ToArray()
                })
                
                .OrderBy(g => g.BarrelLength)
                .ToArray();

            xmlSerializer.Serialize(sw,guns,namespaces);
            return sb.ToString().TrimEnd();

        }
    }
}
