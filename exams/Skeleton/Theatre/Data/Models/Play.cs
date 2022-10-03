using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Theatre.Common;
using Theatre.Data.Models.Enums;

namespace Theatre.Data.Models
{
    public class Play
    {
        public Play()
        {
            this.Casts = new HashSet<Cast>();
            this.Tickets = new HashSet<Ticket>();
        }
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(GlobalConstants.PLAY_TITLE_MAX_LENGTH)]
        public string Title { get; set; }

        
        public TimeSpan Duration { get; set; }//POSSIBLE ERROR

        [MaxLength(10)]
        public float Rating { get; set; }

        public Genre Genre { get; set; }

        [Required]
        [MaxLength(GlobalConstants.PLAY_DESCRIPTION_MAX_LENGTH)]
        public string Description { get; set; }

        [Required]
        [MaxLength(GlobalConstants.PLAY_SCREENWRITER_MAX_LENGTH)]
        public string Screenwriter { get; set; }

        public virtual ICollection<Cast> Casts { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }

    }
}
