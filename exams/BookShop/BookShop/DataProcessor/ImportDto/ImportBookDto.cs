using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;
using BookShop.Common;

namespace BookShop.DataProcessor.ImportDto
{
    [XmlType("Book")]
    public class ImportBookDto
    {
        [XmlElement(nameof(Name))]
        [Required]
        [MinLength(GlobalConstraints.BOOK_NAME_MIN_LENGTH)]
        [MaxLength(GlobalConstraints.BOOK_NAME_MAX_LENGTH)]
        public string Name { get; set; }

        [XmlElement(nameof(Genre))]
        [Range(1,3)]
        public int Genre { get; set; }

        [XmlElement(nameof(Price))]
        [Range(0.01,double.MaxValue)]
        public decimal Price { get; set; }

        [XmlElement(nameof(Pages))]
        [Range(GlobalConstraints.BOOK_PAGES_MIN_LENGTH,GlobalConstraints.BOOK_PAGES_MAX_LENGTH)]
        public int Pages { get; set; }

        [XmlElement(nameof(PublishedOn))]
        [Required]
        public string PublishedOn { get; set; }
    }
}
