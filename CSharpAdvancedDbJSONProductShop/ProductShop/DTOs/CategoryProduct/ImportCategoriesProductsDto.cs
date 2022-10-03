using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace ProductShop.DTOs.CategoryProduct
{
    [JsonObject]
    public  class ImportCategoriesProductsDto
    {
        [JsonProperty(nameof(CategoryId))]
        public int CategoryId { get; set; }
        
        [JsonProperty(nameof(ProductId))]
        public int ProductId { get; set; }
    }
}
