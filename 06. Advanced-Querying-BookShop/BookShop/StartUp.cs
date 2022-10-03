using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using BookShop.Models;
using BookShop.Models.Enums;

namespace BookShop
{
    using Data;
    using Initializer;

    public class StartUp
    {
        public static void Main()
        {
            using var db = new BookShopContext();
            DbInitializer.ResetDatabase(db);

            Console.WriteLine(RemoveBooks(db));
            
        }

        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            AgeRestriction ageRestriction = Enum.Parse<AgeRestriction>(command, true);

            StringBuilder sb=new StringBuilder();
            var books = context.Books
                .Where(b => b.AgeRestriction==ageRestriction )
                .ToList()
                .Select(b => new
                {
                    BookTitle = b.Title
                })
                .OrderBy(b=>b.BookTitle)
                .ToList();

            foreach (var book in books)
            {
                sb.AppendLine(book.BookTitle);
            }
            return sb.ToString().TrimEnd();
        }

        public static string GetGoldenBooks(BookShopContext context)
        {
            EditionType editionType = Enum.Parse<EditionType>("Gold", true);
            var books = context.Books
                .Where(b => b.Copies < 5000&&b.EditionType==editionType)
                .Select(b => new
                {
                    BookId = b.BookId,
                    BookTitle = b.Title
                })
                .OrderBy(b => b.BookId)
                .ToList();
            StringBuilder sb=new StringBuilder();
            foreach (var book in books)
            {
                sb.AppendLine(book.BookTitle);
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetBooksByPrice(BookShopContext context)
        {
            var books = context.Books
                .Where(b => b.Price > 40)
                .Select(b => new
                {
                    BookTitle = b.Title,
                    BookPrice = b.Price
                })
                .OrderByDescending(b => b.BookPrice)
                .ToList();
            var sb=new StringBuilder();
            foreach (var book in books)
            {
                sb.AppendLine($"{book.BookTitle} - ${book.BookPrice:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {
            var books = context.Books
                .Where(b => b.ReleaseDate.Value.Year != year)
                .Select(b => new
                {
                    BookId = b.BookId,
                    BookTitle =
                        b.Title
                })
                .OrderBy(b => b.BookId)
                .ToList();
            var sb=new StringBuilder();
            foreach (var book in books)
            {
                sb.AppendLine(book.BookTitle);
            }

            return sb.ToString().TrimEnd();
        }
        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            var categories = input.Split(" ", StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.ToLower())
                .ToArray();

            var books = context.Books
                .Where(x => x.BookCategories
                    .Any(x => categories.Contains
                        (x.Category.Name.ToLower())))
                .Select(x => x.Title)
                .OrderBy(title => title)
                .ToList();

                return String.Join(Environment.NewLine, books);
            }

        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            DateTime dateTime = DateTime.ParseExact(date, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
            var books = context.Books
                .Where(b => b.ReleaseDate < dateTime)
                .Select(b => new
                {
                    BookTitle = b.Title,
                    BookPrice = b.Price,
                    BookEdition=b.EditionType.ToString(),
                    ReleaseDate = b.ReleaseDate
                })
                .OrderByDescending(b => b.ReleaseDate)
                .ToList();

            StringBuilder sb = new StringBuilder();
            foreach (var book in books)
            {
                sb.AppendLine($"{book.BookTitle} - {book.BookEdition} - ${book.BookPrice:f2}");
            }
            return sb.ToString().TrimEnd();
        }

        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            var authors = context.Authors
                .Where(a => a.FirstName.EndsWith(input))
                .Select(a => new
                {
                    FirsName=a.FirstName,
                    LastName=a.LastName
                })
                .OrderBy(a => a.FirsName)
                .ThenBy(a=>a.LastName)
                .ToList();

            StringBuilder sb=new StringBuilder();
            foreach (var author in authors)
            {
                sb.AppendLine($"{author.FirsName} {author.LastName}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            var books = context.Books
                .Where(b => b.Title.ToLower().Contains(input.ToLower()))
                .Select(b => new
                {
                    BookTitle = b.Title
                })
                .OrderBy(b => b.BookTitle)
                .ToList();

            return $"{string.Join(Environment.NewLine, books.Select(b => b.BookTitle))}";
        }

        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            var books = context.Books
                .Where(b => b.Author.LastName.ToLower().StartsWith(input.ToLower()))
                .Select(b => new
                {
                    BookId = b.BookId,
                    BookTitle = b.Title,
                    AuthorFirstName = b.Author.FirstName,
                    AuthorLastName = b.Author.LastName,
                })
                .OrderBy(b => b.BookId)
                .ToList();
            StringBuilder sb=new StringBuilder();
            foreach (var book in books)
            {
                sb.AppendLine($"{book.BookTitle} ({book.AuthorFirstName} {book.AuthorLastName})");
            }
            return sb.ToString().TrimEnd();
        }

        public static int CountBooks(BookShopContext context, int lengthCheck)
        {
            var books = context.Books
                .Where(b => b.Title.Length > lengthCheck)
                .ToList();
            return books.Count();
        }

        public static string CountCopiesByAuthor(BookShopContext context)
        {
            var authors = context.Authors
                .Select(a => new
                {
                   
                    FullName=a.FirstName + " "+a.LastName,
                    Copies = a.Books
                        .Select(b => b.Copies)
                        .Sum()
                })
                .OrderByDescending(a => a.Copies)
                .ToList();
            return
                $"{string.Join(Environment.NewLine, authors.Select(a => $"{a.FullName} - {a.Copies}"))}";

        }

        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            var categories = context.Categories
                .Select(c => new
                {
                    CategoryName = c.Name,
                    Profit = c.CategoryBooks.Select(b => b.Book.Copies * b.Book.Price).Sum()
                })
                .OrderByDescending(c => c.Profit)
                .ThenBy(c=>c.CategoryName)
                .ToList();
            return $"{string.Join(Environment.NewLine, categories.Select(c => $"{c.CategoryName} ${c.Profit:f2}"))}";
        }

        public static string GetMostRecentBooks(BookShopContext context)
        {
            var categories = context.Categories
                .OrderBy(c => c.Name)
                .Select(c => new
                {
                    CategoryName = c.Name,
                    Books = c.CategoryBooks
                        .Select(b => new
                        {
                            BookName = b.Book.Title,
                            Year = b.Book.ReleaseDate
                        })
                        .OrderByDescending(b => b.Year)
                        .Take(3)
                        .ToList()
                })
                .ToList();
            var sb=new StringBuilder();
            foreach (var category in categories)
            {
                sb.AppendLine($"--{category.CategoryName}");
                foreach (var book in category.Books)
                {
                    sb.AppendLine($"{book.BookName} ({book.Year.Value.Year})");
                }
            }
            return sb.ToString().TrimEnd();

        }

        public static void IncreasePrices(BookShopContext context)
        {
            var books = context.Books
                .Where(b => b.ReleaseDate.Value.Year < 2010)
                .ToList();
            foreach (var book in books)
            {
                book.Price += 5;
            }

            context.SaveChanges();
        }

        public static int RemoveBooks(BookShopContext context)
        {
            var books = context.Books
                .Where(b => b.Copies < 4200)
                .ToList();

            int count= books.Count;
            context.Books.RemoveRange(books);

            context.SaveChanges();
            return count;
        }



    }
}
