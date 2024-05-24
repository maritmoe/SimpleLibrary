namespace Library.DTOs
{
    public record CreateUserPayload(string Name);
    public record BookPayload(string Title, int Pages);
    public record CreateBorrowingPayload(string userId, int bookId, DateOnly borrowedDate);

}