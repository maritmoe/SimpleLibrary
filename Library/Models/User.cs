using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Library.Enums;
using Microsoft.AspNetCore.Identity;

namespace Library.Models
{

    [Table("users")]
    public class User : IdentityUser
    {
        [Column("name")]
        [Required]
        public string Name { get; set; }

        [Column("role")]
        public Role Role { get; set; }

        public ICollection<Borrowing> Borrowings { get; set; }
    }
}