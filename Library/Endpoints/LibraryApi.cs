using Microsoft.AspNetCore.Mvc;
using Library.DTOs;
using Library.Models;
using Library.Repository;

namespace Library.Endpoints
{
    public static class LibraryApi
    {

        public static void ConfigureLibraryApiEndpoint(this WebApplication app)
        {
            var surgeryGroup = app.MapGroup("library");

            surgeryGroup.MapGet("/users", GetUsers);
            surgeryGroup.MapGet("/users/{userId}", GetUser);
            surgeryGroup.MapPost("/users", CreateUser);
            surgeryGroup.MapGet("/books", GetBooks);
            surgeryGroup.MapGet("/books/{bookId}", GetBook);
            surgeryGroup.MapGet("/borrowings/{userId}", GetBorrowings);
            surgeryGroup.MapPost("/borrowings", UserBorrowBook);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        public static async Task<IResult> GetUsers(IRepository repository)
        {
            var users = await repository.GetUsers();
            var userDto = new List<UserResponseDTO>();
            foreach (User user in users)
            {
                userDto.Add(new UserResponseDTO(user));
            }
            return TypedResults.Ok(userDto);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public static async Task<IResult> GetUser(IRepository repository, int userId)
        {
            User? user = await repository.GetUser(userId, PreloadPolicy.PreloadRelations);
            if (user == null)
            {
                return Results.NotFound("User not found");
            }
            var userDto = new UserResponseDTO(user);
            return TypedResults.Ok(userDto);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public static async Task<IResult> CreateUser(CreateUserPayload payload, IRepository repository)
        {
            // validate: a) payload has all of the properties we need (ie. they are NOT null)
            if (payload.Name == null || payload.Name == "")
            {
                return Results.BadRequest("A non-empty Name is required");
            }
            // validate: b) payload properties have acceptable values (ie. not empty, or within required ranges, etc...)

            User? user = await repository.CreateUser(payload.Name);
            if (user == null)
            {
                return Results.BadRequest("Failed to create user.");
            }

            return TypedResults.Ok(new UserResponseDTO(user));
        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        public static async Task<IResult> GetBooks(IRepository repository)
        {
            // GetBooks returns a List<Book> with each Book loading a List<Borrowing>, with each Borrowing loading the User ->points back to Borrowings -> Books
            var books = await repository.GetBooks();
            var bookDto = new List<BookResponseDTO>();
            foreach (Book book in books)
            {
                bookDto.Add(new BookResponseDTO(book));
            }
            return TypedResults.Ok(bookDto);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public static async Task<IResult> GetBook(IRepository repository, int bookId)
        {
            Book? book = await repository.GetBook(bookId, PreloadPolicy.PreloadRelations);
            if (book == null)
            {
                return Results.NotFound("Book not found");
            }
            var bookDto = new BookResponseDTO(book);
            return TypedResults.Ok(bookDto);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        public static async Task<IResult> GetBorrowings(IRepository repository, int userId)
        {
            var borrowings = await repository.GetBorrowings(userId);
            var borrowingDto = new List<BorrowingDTO>();
            foreach (Borrowing borrowing in borrowings)
            {
                borrowingDto.Add(new BorrowingDTO(borrowing));
            }
            return TypedResults.Ok(borrowingDto);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public static async Task<IResult> UserBorrowBook(CreateBorrowingPayload payload, IRepository repository)
        {
            // a) check for null properties on my payload => return bad request if missing fields
            if (payload.userId == null || payload.bookId == null || payload.borrowedDate == null)
            {
                return Results.BadRequest("All fields userId, bookId and borrowedDate are required");
            }

            // b) try to get user; return not found if null
            User? user = await repository.GetUser(payload.userId);
            if (user == null)
            {
                return Results.NotFound("User not found");
            }
            // c) try to get book; return not found if null
            Book? book = await repository.GetBook(payload.bookId);
            if (book == null)
            {
                return Results.NotFound("Book not found");
            }

            Borrowing? borrowing = await repository.CreateBorrowing(payload.borrowedDate, user, book);
            if (borrowing == null)
            {
                return Results.BadRequest("Failed to create borrowing; check if user and book exist.");
            }

            return TypedResults.Ok(new BorrowingDTO(borrowing));

        }
    }
}