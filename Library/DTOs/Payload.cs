namespace Library.DTOs
{
    public record CreateUserPayload(string Name);
    public record CreateBorrowingPayload(int userId, int bookId, DateOnly borrowedDate);

}