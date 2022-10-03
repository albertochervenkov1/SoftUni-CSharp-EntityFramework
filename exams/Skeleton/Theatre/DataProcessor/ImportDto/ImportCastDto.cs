using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;
using Theatre.Common;

namespace Theatre.DataProcessor.ImportDto
{
    [XmlType("Cast")]
    public class ImportCastDto
    {
        [XmlElement(nameof(FullName))]
        [Required]
        [MinLength(GlobalConstants.CAST_FULLNAME_MIN_LENGTH)]
        [MaxLength(GlobalConstants.CAST_FULLNAME_MAX_LENGTH)]
        public string FullName { get; set; }

        [XmlElement(nameof(IsMainCharacter))]
        [Required]
        public string IsMainCharacter { get; set; }

        [XmlElement(nameof(PhoneNumber))]
        [Required]
        [RegularExpression(GlobalConstants.CAST_PHONENUMBER_REGEX)]
        public string PhoneNumber { get; set; }

        [XmlElement(nameof(PlayId))]
        public int PlayId { get; set; }
    }
}
