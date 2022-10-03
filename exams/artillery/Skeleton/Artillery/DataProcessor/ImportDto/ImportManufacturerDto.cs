using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;
using Artillery.Common;

namespace Artillery.DataProcessor.ImportDto
{
    [XmlType("Manufacturer")]
    public class ImportManufacturerDto
    {
        [XmlElement(nameof(ManufacturerName))]
        [Required]
        [MinLength(GlobalConstants.MANUFACTURER_NAME_MIN_LENGTH)]
        [MaxLength(GlobalConstants.MANUFACTURER_NAME_MAX_LENGTH)]
        public string ManufacturerName { get; set; }

        [XmlElement(nameof(Founded))]
        [Required]
        [MinLength(GlobalConstants.MANUFACTURER_FOUNDED_MIN_LENGTH)]
        [MaxLength(GlobalConstants.MANUFACTURER_FOUNDED_MAX_LENGTH)]
        public string Founded { get; set; }
    }
}
