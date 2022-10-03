using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Newtonsoft.Json;
using TeisterMask.Common;

namespace TeisterMask.DataProcessor.ImportDto
{
    [JsonObject]
    public class ImportEmployeeDto
    {
        [Required]
        [MinLength(GlobalConstants.EMPLOYEE_USERNAME_MIN_LENGTH)]
        [MaxLength(GlobalConstants.EMPLOYEE_USERNAME_MAX_LENHTH)]
        [RegularExpression(GlobalConstants.EMPLOYEE_USERNAME_REGEX)]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [RegularExpression(GlobalConstants.EMPLOYEE_PHONE_REGEX)]
        public string Phone { get; set; }

        public int[] Tasks { get; set; }
    }
}
