using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Models
{

    [Table("borrowings")]
    public class Borrowing
    {
        [Column("borrowed_date")]
        public DateOnly BorrowedDate { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        public User User { get; set; }
        [Column("book_id")]
        public int BookId { get; set; }
        public Book Book { get; set; }
    }
}