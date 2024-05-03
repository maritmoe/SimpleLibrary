using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Models
{
    [Table("books")]
    public class Book
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("title")]
        public string Title { get; set; }

        [Column("pages")]
        public int? Pages { get; set; }

        public ICollection<Borrowing> Borrowings { get; set; }
    }
}