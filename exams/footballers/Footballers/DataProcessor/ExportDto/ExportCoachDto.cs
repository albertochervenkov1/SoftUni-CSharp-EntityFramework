using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Footballers.DataProcessor.ExportDto
{
    [XmlType("Coach")]
    public class ExportCoachDto
    {
        [XmlAttribute(nameof(FootballersCount))]
        public int FootballersCount { get; set; }

        [XmlElement(nameof(CoachName))]
        public string CoachName { get; set; }

        [XmlArray(nameof(Footballers))]
        public ExportCoachFootballerDto[] Footballers { get; set; }
    }
}
