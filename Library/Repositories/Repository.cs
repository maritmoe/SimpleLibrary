using Microsoft.EntityFrameworkCore;
using Library.Data;
using Library.Models;

namespace Library.Repository
{
    public class Repository : IRepository
    {
        private readonly DatabaseContext _context;

        public Repository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            return await _context.Users.Include(s => s.Borrowings).ThenInclude(e => e.Book).ToListAsync();
        }

        public async Task<User?> GetUser(int userId, PreloadPolicy preloadPolicy = PreloadPolicy.DoNotPreloadRelations)
        {

            switch (preloadPolicy)
            {
                case PreloadPolicy.PreloadRelations:
                    return await _context.Users.Include(s => s.Borrowings).ThenInclude(e => e.Book).FirstOrDefaultAsync(s => s.Id == userId);
                case PreloadPolicy.DoNotPreloadRelations:
                    return await _context.Users.FirstOrDefaultAsync(s => s.Id == userId);
                default:
                    return null;
            }
        }

        public async Task<User?> CreateUser(string name)
        {
            if (name == "") return null;
            var user = new User { Name = name };
            await _context.Users.AddAsync(user);
            return user;
        }

        public async Task<IEnumerable<Book>> GetBooks()
        {
            return await _context.Books.Include(s => s.Borrowings).ThenInclude(e => e.User).ToListAsync();
        }

        public async Task<Book?> GetBook(int bookId, PreloadPolicy preloadPolicy = PreloadPolicy.DoNotPreloadRelations)
        {

            switch (preloadPolicy)
            {
                case PreloadPolicy.PreloadRelations:
                    return await _context.Books.Include(c => c.Borrowings).ThenInclude(e => e.User).FirstOrDefaultAsync(c => c.Id == bookId);
                case PreloadPolicy.DoNotPreloadRelations:
                    return await _context.Books.FirstOrDefaultAsync(c => c.Id == bookId);
                default:
                    return null;
            }
        }

        public async Task<IEnumerable<Borrowing>> GetBorrowings(int userId)
        {
            return await _context.Borrowings.Where(e => e.UserId == userId).Include(s => s.User).Include(e => e.Book).ToListAsync();
        }

        public async Task<Borrowing?> CreateBorrowing(DateOnly borrowedDate, User user, Book book)
        {
            if (user == null || book == null) return null;
            var borrowing = new Borrowing
            {
                BorrowedDate = borrowedDate,
                UserId = user.Id,
                User = user,
                BookId = book.Id,
                Book = book
            };

            try
            {
                await _context.Borrowings.AddAsync(borrowing);
            }
            catch (Exception ex)
            {
                // failed to create borrowing, maybe the ids are wrong for user or book
                return null;
            }
            return borrowing;
        }
    }
}