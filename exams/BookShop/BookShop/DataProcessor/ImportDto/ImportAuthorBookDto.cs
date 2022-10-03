using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace BookShop.DataProcessor.ImportDto
{
    [JsonObject]
    public class ImportAuthorBookDto
    {
        public int? Id { get; set; }
    }
}
