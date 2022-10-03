using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using BookShop.Common;
using Newtonsoft.Json;

namespace BookShop.DataProcessor.ImportDto
{
    [JsonObject]
    public class ImportAuthorDto
    {
        [Required]
        [MinLength(GlobalConstraints.AUTHOR_FIRSTNAME_MIN_LENGTH)]
        [MaxLength(GlobalConstraints.AUTHOR_FIRSTNAME_MAX_LENGTH)]
        public string FirstName { get; set; }

        [Required]
        [MinLength(GlobalConstraints.AUTHOR_LASTNAME_MIN_LENGTH)]
        [MaxLength(GlobalConstraints.AUTHOR_LASTTNAME_MAX_LENGTH)]

        public string LastName { get; set; }

        [Required]
        [RegularExpression(GlobalConstraints.AUTHOR_PHONE_REGEX)]
        public string Phone { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public ImportAuthorBookDto[] Books { get; set; }

    }
}
