using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;
using TeisterMask.Common;

namespace TeisterMask.DataProcessor.ImportDto
{
    [XmlType("Task")]
    public class ImportProjectTaskDto
    {
        [XmlElement("Name")]
        [Required]
        [MinLength(GlobalConstants.TASK_NAME_MIN_LENHTH)]
        [MaxLength(GlobalConstants.TASK_NAME_MAX_LENGTH)]
        public string Name { get; set; }

        [XmlElement("OpenDate")]
        [Required]
        public string TaskOpenDate { get; set; }

        [XmlElement("DueDate")]
        [Required]
        public string TaskDueDate { get; set; }

        [XmlElement(nameof(ExecutionType))]
        [Range(0,3)]
        public int ExecutionType { get; set; }

        [XmlElement(nameof(LabelType))]
        [Range(0,4)]
        public int LabelType { get; set; }
    }
}
