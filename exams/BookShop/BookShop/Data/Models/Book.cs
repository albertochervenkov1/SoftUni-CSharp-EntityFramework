using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using BookShop.Common;
using BookShop.Data.Models.Enums;

namespace BookShop.Data.Models
{
    public class Book
    {
        public Book()
        {
            this.AuthorsBooks = new HashSet<AuthorBook>();
        }
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(GlobalConstraints.BOOK_NAME_MAX_LENGTH)]
        public string Name { get; set; }

        public Genre Genre { get; set; }

        public decimal Price { get; set; }

        [MaxLength(GlobalConstraints.BOOK_PAGES_MAX_LENGTH)]
        public int Pages { get; set; }

        public DateTime PublishedOn { get; set; }

        public virtual ICollection<AuthorBook> AuthorsBooks { get; set; }
    }
}
