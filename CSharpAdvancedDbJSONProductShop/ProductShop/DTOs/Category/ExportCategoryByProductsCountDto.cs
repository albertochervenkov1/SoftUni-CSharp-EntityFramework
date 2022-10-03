using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace ProductShop.DTOs.Category
{
    [JsonObject]
    public class ExportCategoryByProductsCountDto
    {
        [JsonProperty("category")]
        public string CategoryName { get; set; }
        [JsonProperty("productsCount")]
        public int ProductsCount { get; set; }
        [JsonProperty("averagePrice")]
        public decimal AveragePrice { get; set; }
        [JsonProperty("totalRevenue")]
        public decimal TotalRevenue { get; set; }
    }
}
