using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;
using Artillery.Common;

namespace Artillery.DataProcessor.ImportDto
{
    [XmlType("Country")]
    public class ImportCountryDto
    {
        [XmlElement(nameof(CountryName))]
        [Required]
        [MinLength(GlobalConstants.COUNTRY_NAME_MIN_LENGTH)]
        [MaxLength(GlobalConstants.COUNTRY_NAME_MAX_LENGTH)]
        public string CountryName { get; set; }

        [XmlElement(nameof(ArmySize))]
        [Range(GlobalConstants.COUNTRY_ARMYSZ_MIN_LENGTH,GlobalConstants.COUNTRY_ARMYSZ_MAX_LENGTH)]
        public int ArmySize { get; set; }
    }
}
