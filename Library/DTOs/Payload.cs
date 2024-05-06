namespace Library.DTOs
{
    public record CreateUserPayload(string Name);
    public record BookPayload(string Title, int Pages);
    public record CreateBorrowingPayload(int userId, int bookId, DateOnly borrowedDate);

}