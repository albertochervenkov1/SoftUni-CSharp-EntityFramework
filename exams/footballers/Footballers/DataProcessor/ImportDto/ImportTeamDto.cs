using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Footballers.Common;
using Newtonsoft.Json;

namespace Footballers.DataProcessor.ImportDto
{
    [JsonObject]
    public class ImportTeamDto
    {
        [Required]
        [MinLength(GlobalConstraints.TEAM_NAME_MIN_LENGTH)]
        [MaxLength(GlobalConstraints.TEAM_NAME_MAX_LENGTH)]
        [RegularExpression(GlobalConstraints.TEAM_NAME_REGEX)]
        public string Name { get; set; }

        [Required]
        [MinLength(GlobalConstraints.TEAM_NAT_MIN_LENGTH)]
        [MaxLength(GlobalConstraints.TEAM_NAT_MAX_LENGTH)]
        public string Nationality { get; set; }

        public int Trophies { get; set; }

        public int[] Footballers { get; set; }
    }
}
