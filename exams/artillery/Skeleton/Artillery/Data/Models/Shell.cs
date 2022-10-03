using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Artillery.Common;

namespace Artillery.Data.Models
{
    public class Shell
    {
        public Shell()
        {
            this.Guns=new HashSet<Gun>();
        }
        [Key]
        public int Id { get; set; }

        [MaxLength(1680)]
        public double ShellWeight { get; set; }

        [Required]
        [MaxLength(GlobalConstants.SHELL_CALIBER_MAX_LENGTH)]
        public string Caliber { get; set; }

        public virtual ICollection<Gun> Guns { get; set; }
    }
}
