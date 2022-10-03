using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;
using Theatre.Common;

namespace Theatre.DataProcessor.ImportDto
{
    [XmlType("Play")]
    public class ImportPlayDto
    {
        [XmlElement(nameof(Title))]
        [Required]
        [MinLength(GlobalConstants.PLAY_TITLE_MIN_LENGTH)]
        [MaxLength(GlobalConstants.PLAY_TITLE_MAX_LENGTH)]
        public string Title { get; set; }

        [XmlElement(nameof(Duration))]
        [Required]
        public string Duration { get; set; }

        [XmlElement(nameof(Rating))]
        [Required]
        
        public string Rating { get; set; }

        [XmlElement(nameof(Genre))]
        [Required]
        public string Genre { get; set; }//possible error

        [XmlElement(nameof(Description))]
        [Required]
        [MaxLength(GlobalConstants.PLAY_DESCRIPTION_MAX_LENGTH)]
        public string Description { get; set; }

        [XmlElement(nameof(Screenwriter))]
        [Required]
        [MinLength(GlobalConstants.PLAY_SCREENWRITER_MIN_LENGTH)]
        [MaxLength(GlobalConstants.PLAY_SCREENWRITER_MAX_LENGTH)]
        public string Screenwriter { get; set; }

    }
}
