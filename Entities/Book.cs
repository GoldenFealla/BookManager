using System.ComponentModel.DataAnnotations;
using NpgsqlTypes;

using BookManager.Dtos;

namespace BookManager.Entities
{
    public class Book
    {
        [Key]
        public required string ISBN { get; set; }
        public required string Title { get; set; }
        public required string Author { get; set; }
        public int YearOfPublication { get; set; }
        public required string Publisher { get; set; }
        public required string ImageURLS { get; set; }
        public required string ImageURLM { get; set; }
        public required string ImageURLL { get; set; }
        public NpgsqlTsVector? SearchVector { get; set; }

        public BookDto ToBookDto()
        {
            return new BookDto()
            {
                ISBN = ISBN,
                Title = Title,
                Author = Author,
                YearOfPublication = YearOfPublication,
                Publisher = Publisher,
                ImageURLS = ImageURLS,
                ImageURLM = ImageURLM,
                ImageURLL = ImageURLL
            };
        }
    }
}
