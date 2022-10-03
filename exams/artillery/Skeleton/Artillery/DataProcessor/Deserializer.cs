using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Artillery.Data.Models;
using Artillery.Data.Models.Enums;
using Artillery.DataProcessor.ImportDto;
using Newtonsoft.Json;

namespace Artillery.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Artillery.Data;

    public class Deserializer
    {
        private const string ErrorMessage =
                "Invalid data.";
        private const string SuccessfulImportCountry =
            "Successfully import {0} with {1} army personnel.";
        private const string SuccessfulImportManufacturer =
            "Successfully import manufacturer {0} founded in {1}.";
        private const string SuccessfulImportShell =
            "Successfully import shell caliber #{0} weight {1} kg.";
        private const string SuccessfulImportGun =
            "Successfully import gun {0} with a total weight of {1} kg. and barrel length of {2} m.";

        public static string ImportCountries(ArtilleryContext context, string xmlString)
        {
            StringBuilder sb=new StringBuilder();

            XmlRootAttribute xmlRoot = new XmlRootAttribute("Countries");
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportCountryDto[]), xmlRoot);

            using StringReader sr = new StringReader(xmlString);

            ImportCountryDto[] countryDtos = (ImportCountryDto[])xmlSerializer.Deserialize(sr);
            HashSet<Country> countries=new HashSet<Country>();

            foreach (var countryDto in countryDtos)
            {
                if (!IsValid(countryDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Country country = new Country()
                {
                    CountryName = countryDto.CountryName,
                    ArmySize = countryDto.ArmySize
                };
                countries.Add(country);
                sb.AppendLine(String.Format(SuccessfulImportCountry, country.CountryName, country.ArmySize));
            }
            context.Countries.AddRange(countries);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportManufacturers(ArtilleryContext context, string xmlString)
        {
            StringBuilder sb=new StringBuilder();

            XmlRootAttribute xmlRoot = new XmlRootAttribute("Manufacturers");
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportManufacturerDto[]), xmlRoot);

            using StringReader sr=new StringReader(xmlString);

            ImportManufacturerDto[] manufacturerDtos = (ImportManufacturerDto[])xmlSerializer.Deserialize(sr);
            HashSet<Manufacturer> manufacturers=new HashSet<Manufacturer>();

            foreach (var manufacturerDto in manufacturerDtos)
            {
                if (!IsValid(manufacturerDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (manufacturers.Any(m => m.ManufacturerName == manufacturerDto.ManufacturerName))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Manufacturer manufacturer = new Manufacturer()
                {
                    ManufacturerName = manufacturerDto.ManufacturerName,
                    Founded = manufacturerDto.Founded
                };
                manufacturers.Add(manufacturer);

                string[] founded = manufacturer.Founded.Split(", ");
                string[] arr = founded.Reverse().Take(2).Reverse().ToArray();
                string data = $"{arr[0]}, {arr[1]}";
                sb.AppendLine(String.Format(SuccessfulImportManufacturer, manufacturer.ManufacturerName, data));
            }
            context.Manufacturers.AddRange(manufacturers);
            context.SaveChanges();

            return sb.ToString();
        }

        public static string ImportShells(ArtilleryContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            XmlRootAttribute xmlRoot = new XmlRootAttribute("Shells");
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportShellDto[]), xmlRoot);

            using StringReader sr=new StringReader(xmlString);

            ImportShellDto[] shellDtos = (ImportShellDto[])xmlSerializer.Deserialize(sr);
            HashSet<Shell> shells = new HashSet<Shell>();

            foreach (var shellDto in shellDtos)
            {
                if (!IsValid(shellDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                Shell shell = new Shell()
                {
                    ShellWeight = shellDto.ShellWeight,
                    Caliber = shellDto.Caliber
                };
                shells.Add(shell);
                sb.AppendLine(String.Format(SuccessfulImportShell, shell.Caliber, shell.ShellWeight));
            }
            context.Shells.AddRange(shells);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportGuns(ArtilleryContext context, string jsonString)
        {
            StringBuilder sb=new StringBuilder();
            ImportGunDto[] gunDtos = JsonConvert.DeserializeObject<ImportGunDto[]>(jsonString);

            HashSet<Gun> guns = new HashSet<Gun>();

            foreach (var gunDto in gunDtos)
            {
                if (!IsValid(gunDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                bool isEnumValid = Enum.TryParse<GunType>(gunDto.GunType, out GunType gunType);
                if (!isEnumValid)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Gun gun = new Gun()
                {
                    ManufacturerId = gunDto.ManufacturerId,
                    GunWeight = gunDto.GunWeight,
                    BarrelLength = gunDto.BarrelLength,
                    NumberBuild = gunDto.NumberBuild,
                    Range = gunDto.Range,
                    GunType = Enum.Parse<GunType>(gunDto.GunType),
                    
                    ShellId = gunDto.ShellId,
                };
                HashSet<CountryGun> employeeTasks = new HashSet<CountryGun>();
                foreach (var countryDto in gunDto.Countries)
                {
                    Country country = context.Countries
                        .Find(countryDto.Id);
                    
                    CountryGun countryGun = new CountryGun()
                    {
                        Gun = gun,
                        Country = country
                    };

                    employeeTasks.Add(countryGun);
                }

                guns.Add(gun);
                sb.AppendLine(String.Format(SuccessfulImportGun, gun.GunType, gun.GunWeight, gun.BarrelLength));

            }

            context.Guns.AddRange(guns);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }
        private static bool IsValid(object obj)
        {
            var validator = new ValidationContext(obj);
            var validationRes = new List<ValidationResult>();

            var result = Validator.TryValidateObject(obj, validator, validationRes, true);
            return result;
        }
    }
}
