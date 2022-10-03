using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Artillery.DataProcessor.ExportDto
{
    [XmlType("Country")]
    public class ExportGunCountryDto
    {
        [XmlAttribute(nameof(Country))]
        public string Country { get; set; }

        [XmlAttribute(nameof(ArmySize))]
        public string ArmySize { get; set; }
    }
}
