using Library.Models;

namespace Library.DTOs
{
    class UserResponseDTO
    {
        // define all of the properties that we want to return to the client
        public int Id { get; set; }
        public string Name { get; set; }

        public List<UserBorrowingDTO> Borrowings { get; set; } = new List<UserBorrowingDTO>();

        public UserResponseDTO(User user)
        {
            Id = user.Id;
            Name = user.Name;
            if (user.Borrowings != null)
            {
                foreach (Borrowing borrowing in user.Borrowings)
                {
                    Borrowings.Add(new UserBorrowingDTO(borrowing));
                }
            }
        }
    }

    class BookResponseDTO
    {
        // define all of the properties that we want to return to the client
        public int Id { get; set; }
        public string Title { get; set; }
        public int? Pages { get; set; }

        public List<BookBorrowingDTO> Borrowings { get; set; } = new List<BookBorrowingDTO>();

        public BookResponseDTO(Book book)
        {
            Id = book.Id;
            Title = book.Title;
            Pages = book.Pages;
            if (book.Borrowings != null)
            {
                foreach (Borrowing borrowing in book.Borrowings)
                {
                    Borrowings.Add(new BookBorrowingDTO(borrowing));
                }
            }

        }
    }

    class UserBorrowingDTO
    {
        public DateOnly BorrowedDate { get; set; }

        public BookDTO Book { get; set; }

        public UserBorrowingDTO(Borrowing borrowing)
        {
            BorrowedDate = borrowing.BorrowedDate;
            Book = new BookDTO(borrowing.Book);
        }
    }

    class BookBorrowingDTO
    {
        public DateOnly BorrowedDate { get; set; }

        public UserDTO User { get; set; }

        public BookBorrowingDTO(Borrowing borrowing)
        {
            BorrowedDate = borrowing.BorrowedDate;
            User = new UserDTO(borrowing.User);
        }
    }
    class BorrowingDTO
    {
        public DateOnly BorrowedDate { get; set; }

        public UserDTO User { get; set; }
        public BookDTO Book { get; set; }

        public BorrowingDTO(Borrowing borrowing)
        {
            BorrowedDate = borrowing.BorrowedDate;
            User = new UserDTO(borrowing.User);
            Book = new BookDTO(borrowing.Book);
        }
    }

    class BookDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int? Pages { get; set; }

        public BookDTO(Book book)
        {
            Id = book.Id;
            Title = book.Title;
            Pages = book.Pages;
        }
    }

    class UserDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public UserDTO(User user)
        {
            Id = user.Id;
            Name = user.Name;
        }
    }

}