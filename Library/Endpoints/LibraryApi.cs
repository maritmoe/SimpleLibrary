using Microsoft.AspNetCore.Mvc;
using Library.DTOs;
using Library.Models;
using Library.Repository;
using Microsoft.AspNetCore.Authorization;

namespace Library.Endpoints
{
    public static class LibraryApi
    {

        public static void ConfigureLibraryApiEndpoint(this WebApplication app)
        {
            var libraryGroup = app.MapGroup("library");

            libraryGroup.MapGet("/users", GetUsers);
            libraryGroup.MapGet("/users/{userId}", GetUser);
            libraryGroup.MapPost("/users", CreateUser); // TODO: delete?
            libraryGroup.MapPut("/users/{userId}", UpdateUser);
            libraryGroup.MapDelete("/users/{userId}", DeleteUser);
            libraryGroup.MapGet("/books", GetBooks);
            libraryGroup.MapGet("/books/{bookId}", GetBook);
            libraryGroup.MapPost("/books", CreateBook);
            libraryGroup.MapPost("/books/multiple", CreateBooks);
            libraryGroup.MapPut("/books/{bookId}", UpdateBook);
            libraryGroup.MapGet("/borrowings/{userId}", GetUserBorrowings);
            libraryGroup.MapGet("/borrowings", GetAllBorrowings);
            libraryGroup.MapPost("/borrowings", UserBorrowBook);
        }


        [Authorize(Roles = "Admin")]
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


        // TODO: Check if authorized user has id userId or is admin
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public static async Task<IResult> GetUser(IRepository repository, string userId)
        {
            User? user = await repository.GetUser(userId, PreloadPolicy.PreloadRelations);
            if (user == null)
            {
                return Results.NotFound("User not found");
            }
            var userDto = new UserResponseDTO(user);
            return TypedResults.Ok(userDto);
        }


        //TODO: Delete?
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public static async Task<IResult> CreateUser(CreateUserPayload payload, IRepository repository)
        {
            // validate payload has all of the properties needed
            if (payload.Name == null || payload.Name == "")
            {
                return Results.BadRequest("A non-empty Name is required");
            }

            User? user = await repository.CreateUser(payload.Name);
            if (user == null)
            {
                return Results.BadRequest("Failed to create user.");
            }

            return TypedResults.Ok(new UserResponseDTO(user));
        }


        // TODO: Check if authorized user has id userId or is admin
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public static async Task<IResult> UpdateUser(IRepository repository, string userId, CreateUserPayload payload)
        {
            User? user = await repository.GetUser(userId, PreloadPolicy.PreloadRelations);
            if (user == null)
            {
                return Results.NotFound("User not found");
            }
            if (payload.Name == null || payload.Name == "")
            {
                return Results.BadRequest("Name cannot be empty");
            }

            user.Name = payload.Name;

            await repository.UpdateUser(user);
            return TypedResults.Ok(new UserResponseDTO(user));

        }


        // TODO: Check if authorized user has id userId or is admin
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public static async Task<IResult> DeleteUser(IRepository repository, string userId)
        {
            User? user = await repository.DeleteUser(userId);
            if (user == null)
            {
                return Results.NotFound("User not found");
            }
            return TypedResults.Ok(user);
        }


        [AllowAnonymous]
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


        [AllowAnonymous]
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


        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public static async Task<IResult> CreateBook(IRepository repository, BookPayload payload)
        {
            if (payload.Title == null || payload.Title == "" || payload.Pages <= 0)
            {
                return Results.BadRequest("A non-empty Title and positive Pages is required");
            }

            Book? book = await repository.CreateBook(payload.Title, payload.Pages);
            if (book == null)
            {
                return Results.BadRequest("Failed to create book");
            }

            return TypedResults.Ok(new BookResponseDTO(book));

        }


        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public static async Task<IResult> CreateBooks(IRepository repository, List<BookPayload> payloadList)
        {
            var bookDtos = new List<BookDTO>();
            foreach (BookPayload payload in payloadList)
            {
                if (payload.Title == null || payload.Title == "" || payload.Pages <= 0)
                {
                    return Results.BadRequest("A non-empty Title and positive Pages is required");
                }

                Book? book = await repository.CreateBook(payload.Title, payload.Pages);
                if (book == null)
                {
                    return Results.BadRequest($"Failed to create book with title {payload.Title}");
                }
                bookDtos.Add(new BookDTO(book));
            }
            return TypedResults.Ok(bookDtos);

        }


        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public static async Task<IResult> UpdateBook(IRepository repository, int bookId, BookPayload payload)
        {
            Book? book = await repository.GetBook(bookId, PreloadPolicy.PreloadRelations);
            if (book == null)
            {
                return Results.NotFound("Book not found");
            }
            if (payload.Title == null || payload.Title == "")
            {
                return Results.BadRequest("Title cannot be empty");
            }
            if (payload.Pages <= 0)
            {
                return Results.BadRequest("Pages must be a positive number");
            }

            book.Title = payload.Title;
            book.Pages = payload.Pages;

            await repository.UpdateBook(book);
            return TypedResults.Ok(new BookResponseDTO(book));

        }


        // TODO: Check if authorized user has id userId or is admin
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public static async Task<IResult> GetUserBorrowings(IRepository repository, string userId)
        {
            var borrowings = await repository.GetBorrowings(userId);
            var borrowingDto = new List<BorrowingDTO>();
            foreach (Borrowing borrowing in borrowings)
            {
                borrowingDto.Add(new BorrowingDTO(borrowing));
            }
            return TypedResults.Ok(borrowingDto);
        }


        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public static async Task<IResult> GetAllBorrowings(IRepository repository)
        {
            var borrowings = await repository.GetBorrowings();
            var borrowingDto = new List<BorrowingDTO>();
            foreach (Borrowing borrowing in borrowings)
            {
                borrowingDto.Add(new BorrowingDTO(borrowing));
            }
            return TypedResults.Ok(borrowingDto);
        }


        // TODO: Check if authorized user has id userId or is admin
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public static async Task<IResult> UserBorrowBook(CreateBorrowingPayload payload, IRepository repository)
        {
            // try to get user; return not found if null
            User? user = await repository.GetUser(payload.userId);
            if (user == null)
            {
                return Results.NotFound("User not found");
            }
            // try to get book; return not found if null
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