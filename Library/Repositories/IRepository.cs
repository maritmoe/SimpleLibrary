using Library.Models;

namespace Library.Repository
{
    public enum PreloadPolicy
    {
        PreloadRelations,
        DoNotPreloadRelations
    }

    public interface IRepository
    {
        Task<IEnumerable<User>> GetUsers();
        Task<User?> GetUser(int userId, PreloadPolicy preloadPolicy = PreloadPolicy.DoNotPreloadRelations);
        Task<User?> CreateUser(string name);
        Task<User?> DeleteUser(int userId);

        Task<IEnumerable<Book>> GetBooks();
        Task<Book?> GetBook(int bookId, PreloadPolicy preloadPolicy = PreloadPolicy.DoNotPreloadRelations);
        Task<Book?> UpdateBook(Book book);
        Task<IEnumerable<Borrowing>> GetBorrowings(int userId);
        Task<Borrowing?> CreateBorrowing(DateOnly borrowedDate, User user, Book book);
    }
}