using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace ProductShop.DTOs.Category
{
    public class ImportCategoryDto
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
