using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Artillery.DataProcessor.ExportDto
{
    [XmlType("Gun")]
    public class ExportGunDto
    {
        [XmlElement(nameof(Manufacturer))]
        public string Manufacturer { get; set; }

        [XmlElement(nameof(GunType))]
        public string GunType { get; set; }

        [XmlElement(nameof(GunWeight))]
        public string GunWeight { get; set; }

        [XmlElement(nameof(BarrelLength))]
        public string BarrelLength { get; set; }

        [XmlElement(nameof(Range))]
        public string Range { get; set; }

        [XmlArray(nameof(Countries))]
        public ExportGunCountryDto[] Countries { get; set; }
    }
}
