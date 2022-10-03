using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;
using Artillery.Common;

namespace Artillery.DataProcessor.ImportDto
{
    [XmlType("Shell")]
    public class ImportShellDto
    {
        [XmlElement(nameof(ShellWeight))]
        [Range(GlobalConstants.SHELL_WEIGHT_MIN_LENGTH,GlobalConstants.SHELL_WEIGHT_MAX_LENGTH)]
        public double ShellWeight { get; set; }

        [XmlElement(nameof(Caliber))]
        [Required]
        [MinLength(GlobalConstants.SHELL_CALIBER_MIN_LENGTH)]
        [MaxLength(GlobalConstants.SHELL_CALIBER_MAX_LENGTH)]
        public string Caliber { get; set; }
    }
}
