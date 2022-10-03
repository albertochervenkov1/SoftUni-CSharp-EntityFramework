using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace BookShop.DataProcessor.ExportDto
{
    [XmlType("Book")]
    public class ExportBookDto
    {
        [XmlAttribute(nameof(Pages))]
        public int Pages { get; set; }

        [XmlElement(nameof(Name))]
        public string Name { get; set; }

        [XmlElement(nameof(Date))]
        public string Date { get; set; }

    }
}
