using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;
using Footballers.Common;

namespace Footballers.DataProcessor.ImportDto
{
    [XmlType("Footballer")]
    public class ImportCoachFootballerDto
    {
        [XmlElement(nameof(Name))]
        [Required]
        [MinLength(GlobalConstraints.FOTBBALLER_NAME_MIN_LENGTH)]
        [MaxLength(GlobalConstraints.FOTBBALLER_NAME_MAX_LENGTH)]
        public string Name { get; set; }

        [XmlElement(nameof(ContractStartDate))]
        [Required]
        public string ContractStartDate { get; set; }
        [XmlElement(nameof(ContractEndDate))]
        [Required]
        public string ContractEndDate { get; set; }

        [XmlElement(nameof(BestSkillType))]
        [Range(0,4)]
        public int BestSkillType { get; set; }

        [XmlElement(nameof(PositionType))]
        [Range(0,3)]
        public int PositionType { get; set; }
    }
}
