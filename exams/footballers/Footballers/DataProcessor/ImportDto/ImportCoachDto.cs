using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;
using Footballers.Common;

namespace Footballers.DataProcessor.ImportDto
{
    [XmlType("Coach")]
    public class ImportCoachDto
    {
        [XmlElement(nameof(Name))]
        [Required]
        [MinLength(GlobalConstraints.COACH_NAME_MIN_LENGTH)]
        [MaxLength(GlobalConstraints.COACH_NAME_MAX_LENGTH)]
        public string Name { get; set; }

        [XmlElement(nameof(Nationality))]
        [Required]
        public string Nationality { get; set; }

        [XmlArray(nameof(Footballers))]
        public ImportCoachFootballerDto[] Footballers { get; set; }
    }
}
