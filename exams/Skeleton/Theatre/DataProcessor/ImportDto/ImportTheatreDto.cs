using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Newtonsoft.Json;
using Theatre.Common;

namespace Theatre.DataProcessor.ImportDto
{
    [JsonObject]
    public class ImportTheatreDto
    {
        [JsonProperty(nameof(Name))]
        [Required]
        [MinLength(GlobalConstants.THEATRE_NAME_MIN_LENGTH)]
        [MaxLength(GlobalConstants.THEATRE_NAME_MAX_LENGTH)]
        public string Name { get; set; }

        [JsonProperty(nameof(NumberOfHalls))]
        [Required]
        [Range(GlobalConstants.THEATRE_HALLS_MIN_LENGTH,GlobalConstants.THEATRE_HALLS_MAX_LENGTH)]
        public sbyte NumberOfHalls { get; set; }

        [JsonProperty(nameof(Director))]
        [Required]
        [MinLength(GlobalConstants.THEATRE_DIRECTOR_MIN_LENGTH)]
        [MaxLength(GlobalConstants.THEATRE_DIRECTOR_MAX_LENGTH)]
        public string Director { get; set; }

        [JsonProperty(nameof(Tickets))]
        public ImportTheatreTicketsDto[] Tickets { get; set; }

    }
}
