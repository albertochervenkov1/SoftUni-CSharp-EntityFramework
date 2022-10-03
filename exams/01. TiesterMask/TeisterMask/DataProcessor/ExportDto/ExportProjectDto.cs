using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace TeisterMask.DataProcessor.ExportDto
{
    [XmlType("Project")]
    public class ExportProjectDto
    {
        [XmlElement("ProjectName")]
        public string Name { get; set; }

        [XmlAttribute(nameof(TasksCount))]
        public int TasksCount { get; set; }

        [XmlElement(nameof(HasEndDate))]
        public string HasEndDate { get; set; }

        [XmlArray(nameof(Tasks))]
        public ExportProjectTaskDto[] Tasks { get; set; }
    }
}
