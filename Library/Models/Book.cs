using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Models
{
    [Table("books")]
    public class Book
    {
        [Column("id")]
        [Key]
        public int Id { get; set; }
        [Column("title")]
        [Required]
        public string Title { get; set; }

        [Column("pages")]
        [Required]
        public int? Pages { get; set; }

        public ICollection<Borrowing> Borrowings { get; set; }
    }
}