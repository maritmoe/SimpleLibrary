using Microsoft.EntityFrameworkCore;
using Library.Models;

namespace Library.Data
{
    public class DatabaseContext : DbContext
    {
        private string _connectionString;
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
            // loading the DefaultConnectionString value from the appsettings.json
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            _connectionString = configuration.GetValue<string>("ConnectionStrings:DefaultConnectionString")!;
            this.Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // use postgresql db
            optionsBuilder.UseNpgsql(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // set a primary composite key on the borrowings table
            modelBuilder.Entity<Borrowing>().HasKey(e => new { e.BorrowedDate, e.UserId, e.BookId });

            // seed some users
            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Name = "John Doe" },
                new User { Id = 2, Name = "Jane Doe" },
                new User { Id = 3, Name = "John Smith" },
                new User { Id = 4, Name = "Jane Smith" }
            );

            // seed some books
            modelBuilder.Entity<Book>().HasData(
                new Book { Id = 1, Title = "C# Book For Beginners" },
                new Book { Id = 2, Title = "React Volume 2" },
                new Book { Id = 3, Title = "Java" },
                new Book { Id = 4, Title = "How To Use Postgres" }
            );

            // seed some borrowings
            List<Borrowing> borrowings = new List<Borrowing>();
            borrowings.Add(new Borrowing { BorrowedDate = new DateOnly(2024, 01, 01), UserId = 1, BookId = 1 });
            borrowings.Add(new Borrowing { BorrowedDate = new DateOnly(2024, 01, 20), UserId = 2, BookId = 2 });
            borrowings.Add(new Borrowing { BorrowedDate = new DateOnly(2024, 01, 11), UserId = 3, BookId = 4 });
            borrowings.Add(new Borrowing { BorrowedDate = new DateOnly(2024, 01, 15), UserId = 4, BookId = 3 });
            borrowings.Add(new Borrowing { BorrowedDate = new DateOnly(2024, 02, 01), UserId = 2, BookId = 1 });

            // add to model
            modelBuilder.Entity<Borrowing>().HasData(borrowings);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Borrowing> Borrowings { get; set; }
    }
}