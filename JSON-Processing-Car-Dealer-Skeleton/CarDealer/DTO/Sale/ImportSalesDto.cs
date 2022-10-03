using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace CarDealer.DTO.Sale
{
    [JsonObject]
    public class ImportSalesDto
    {
        [JsonProperty("carId")]
        public int CarId { get; set; }
        [JsonProperty("customerId")]
        public int CustomerId { get; set; }
        [JsonProperty("discount")]
        public decimal Discount { get; set; }
    }
}
