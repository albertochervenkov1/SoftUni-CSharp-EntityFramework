using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;
using System.Text;
using Artillery.Common;
using Artillery.Data.Models.Enums;

namespace Artillery.Data.Models
{
    public class Gun
    {
        public Gun()
        {
            this.CountriesGuns = new HashSet<CountryGun>();
        }

        [Key] public int Id { get; set; }

        [ForeignKey(nameof(Manufacturer))] public int ManufacturerId { get; set; }
        public virtual Manufacturer Manufacturer { get; set; }

        [MaxLength(GlobalConstants.GUN_WEIGHT_MAX_LENGTH)]
        public int GunWeight { get; set; }

        [MaxLength(35)] 
        public double BarrelLength { get; set; }

        public int? NumberBuild { get; set; } 

        [MaxLength(GlobalConstants.GUN_RANGE_MAX_LENGTH)]
        public int Range { get; set; }

        public GunType GunType { get; set; }

        [ForeignKey(nameof(Shell))] public int ShellId { get; set; }
        public virtual Shell Shell { get; set; }

        public virtual ICollection<CountryGun> CountriesGuns { get; set; }
    }
}
