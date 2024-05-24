using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Models
{

    [Table("borrowings")]
    public class Borrowing
    {
        [Column("borrowed_date")]
        [Required]
        public DateOnly BorrowedDate { get; set; }
        [Column("user_id")]
        [Required]
        public string UserId { get; set; }
        public User User { get; set; }
        [Column("book_id")]
        [Required]
        public int BookId { get; set; }
        public Book Book { get; set; }
    }
}