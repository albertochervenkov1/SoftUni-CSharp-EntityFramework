using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Artillery.Common;

namespace Artillery.Data.Models
{
    public class Country
    {
        public Country()
        {
            this.CountriesGuns = new HashSet<CountryGun>();
        }
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(GlobalConstants.COUNTRY_NAME_MAX_LENGTH)]
        public string CountryName { get; set; }

        [MaxLength(GlobalConstants.COUNTRY_ARMYSZ_MAX_LENGTH)]
        public int ArmySize { get; set; }

        public virtual ICollection<CountryGun> CountriesGuns { get; set; }
    }
}
