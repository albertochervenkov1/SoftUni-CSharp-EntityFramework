using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Newtonsoft.Json;

namespace Theatre.DataProcessor.ImportDto
{
    [JsonObject]
    public class ImportTheatreTicketsDto
    {
        [JsonProperty(nameof(Price))]
        [Range(1.00,100.00)]
        public decimal Price { get; set; }

        [JsonProperty(nameof(RowNumber))]
        [Range(1,10)]
        public sbyte RowNumber { get; set; }

        [JsonProperty(nameof(PlayId))]
        public int PlayId { get; set; }
    }
}
