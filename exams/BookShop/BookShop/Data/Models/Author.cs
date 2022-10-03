using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using BookShop.Common;

namespace BookShop.Data.Models
{
    public class Author
    {
        public Author()
        {
            this.AuthorsBooks = new HashSet<AuthorBook>();
        }
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(GlobalConstraints.AUTHOR_FIRSTNAME_MAX_LENGTH)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(GlobalConstraints.AUTHOR_LASTTNAME_MAX_LENGTH)]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Phone { get; set; }

        public virtual ICollection<AuthorBook> AuthorsBooks { get; set; }
    }
}
