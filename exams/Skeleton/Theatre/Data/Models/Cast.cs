using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Theatre.Common;

namespace Theatre.Data.Models
{
    public class Cast
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(GlobalConstants.CAST_FULLNAME_MAX_LENGTH)]
        public string FullName { get; set; }

        [Required]
        public bool IsMainCharacter  { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [ForeignKey("Play")]
        public int PlayId { get; set; }
        public virtual Play Play { get; set; }
    }
}
