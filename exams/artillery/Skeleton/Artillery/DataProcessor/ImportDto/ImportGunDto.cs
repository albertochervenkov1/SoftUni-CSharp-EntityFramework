using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Artillery.Common;
using Newtonsoft.Json;

namespace Artillery.DataProcessor.ImportDto
{
    [JsonObject]
    public class ImportGunDto
    {
        
        public int ManufacturerId { get; set; }

       
        [Range(GlobalConstants.GUN_WEIGHT_MIN_LENGTH,GlobalConstants.GUN_WEIGHT_MAX_LENGTH)]
        public int GunWeight { get; set; }

        
        [Range(GlobalConstants.GUN_BARREL_MIN_LENGTH,GlobalConstants.GUN_BARREL_MAX_LENGTH)]
        public double BarrelLength { get; set; }

        public int? NumberBuild { get; set; }

        
        [Range(GlobalConstants.GUN_RANGE_MIN_LENGTH,GlobalConstants.GUN_RANGE_MAX_LENGTH)]
        public int Range { get; set; }

        
        public string GunType { get; set; }

        
        public int ShellId { get; set; }

        public ImportGunCountryDto[] Countries { get; set; }
    }
}
