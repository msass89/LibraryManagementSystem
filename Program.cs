class LibraryManager
{
    private static List<Book> booksInLibrary = new List<Book>();
    private const int maxBooksInLibrary = 5;

    private static List<Book> borrowedBooks = new List<Book>();
    private const int maxBorrowedBooks = 3;


    static void Main()
    {
        while (true)
        {
            // Display the menu options to the user
            DisplayMenu();
            string action = (Console.ReadLine() ?? string.Empty).ToLower();

            // Handle user action
            switch (action)
            {
                case "add":
                    AddBook();
                    break;
                case "remove":
                    RemoveBook();
                    break;
                case "search":
                    SearchBook();
                    break;
                case "borrow":
                    BorrowBook();
                    break;
                case "return":
                    ReturnBook();
                    break;
                case "exit":
                    return;
                default:
                    Console.WriteLine("Invalid action. Please type 'add', 'remove', 'search', 'borrow', 'return', or 'exit'.");
                    break;
            }

            // Display the current list of books in the library
            DisplayLibrary();
        }
    }

    static void DisplayMenu()
    {
        Console.WriteLine("\nWould you like to add, remove, search for, borrow, or return a book? (add/remove/search/borrow/return/exit)");
    }

    static void AddBook()
    {
        // Check if library is full
        if (booksInLibrary.Count >= maxBooksInLibrary)
        {
            Console.WriteLine("The library is full. No more books can be added.");
            return;
        }

        // Prompt user to enter the title of the book to add and add it to the collection
        string newBookTitle = PromptForNonEmptyTitle("Enter the title of the book to add:");
        booksInLibrary.Add(new Book(newBookTitle));
        Console.WriteLine($"'{newBookTitle}' added successfully.");
    }

    static void RemoveBook()
    {
        // Check if library is empty
        if (booksInLibrary.Count == 0)
        {
            Console.WriteLine("The library is empty. No books to remove.");
            return;
        }

        // Prompt user to enter the title of the book to remove and remove it from the collection
        string removeTitleInput = PromptForNonEmptyTitle("Enter the title of the book to remove:");
        Book? bookToRemove = FindBookByTitle(removeTitleInput, booksInLibrary);

        if (bookToRemove != null && booksInLibrary.Remove(bookToRemove))
        {
            Console.WriteLine($"'{removeTitleInput}' removed successfully.");
        }
        else
        {
            Console.WriteLine("Book not found.");
        }
    }

    static void DisplayLibrary()
    {
        Console.WriteLine("\nAvailable books:");

        // Check if library is empty
        if (booksInLibrary.Count == 0)
        {
            Console.WriteLine("  (No books in the library)");
        }
        else
        {
            // Loop through each book in the collection and display it with a dash
            foreach (Book book in booksInLibrary)
            {
                Console.WriteLine($"  - {book.Title} {book.GetBookStatus()}");
            }
        }
    }

    static void SearchBook()
    {
        // Check if library is empty
        if (booksInLibrary.Count == 0)
        {
            Console.WriteLine("The library is empty. No books to search.");
            return;
        }

        // Prompt user to enter the title of the book to search and search for it in the collection
        string searchTitle = PromptForNonEmptyTitle("Enter the title of the book to search:");
        bool bookFound = booksInLibrary.Any(book => string.Equals(book.Title, searchTitle, StringComparison.OrdinalIgnoreCase));

        // Display the search result
        Console.WriteLine(bookFound ? $"'{searchTitle}' is in the library." : $"'{searchTitle}' is not in the library.");
    }

    static void BorrowBook()
    {
        // Check if library is empty
        if (booksInLibrary.Count == 0)
        {
            Console.WriteLine("The library is empty. No books to borrow.");
            return;
        }

        // Check if the user has reached the maximum number of borrowed books
        if (borrowedBooks.Count >= maxBorrowedBooks)
        {
            Console.WriteLine("You have reached the maximum number of borrowed books.");
            return;
        }

        // Prompt user to enter the title of the book to borrow and borrow it from the collection
        string borrowTitleInput = PromptForNonEmptyTitle("Enter the title of the book to borrow:");
        Book? bookToBorrow = FindBookByTitle(borrowTitleInput, booksInLibrary);
        
        if (bookToBorrow != null)
        {
            if (bookToBorrow.IsBorrowed)
            {
                Console.WriteLine($"'{borrowTitleInput}' is already borrowed.");
            }
            else
            {
                bookToBorrow.BorrowBook();
                borrowedBooks.Add(bookToBorrow);
                Console.WriteLine($"'{borrowTitleInput}' borrowed successfully.");
            }
        }
        else
        {
            Console.WriteLine("Book not found.");
        }
    }

    static void ReturnBook()
    {
        // Check if there are any borrowed books
        if (borrowedBooks.Count == 0)
        {
            Console.WriteLine("You have no borrowed books to return.");
            return;
        }

        // Prompt user to enter the title of the book to return
        string returnTitleInput = PromptForNonEmptyTitle("Enter the title of the book to return:");
        Book? bookToReturn = FindBookByTitle(returnTitleInput, borrowedBooks);
       
        if (bookToReturn != null)
        {
            bookToReturn.ReturnBook();
            borrowedBooks.Remove(bookToReturn);
            Console.WriteLine($"'{returnTitleInput}' returned successfully.");
        }
        else
        {
            Console.WriteLine("Book not found in borrowed books.");
        }
    }

    // Helper: prompt until user enters a non-empty trimmed title
    private static string PromptForNonEmptyTitle(string prompt)
    {
        string? input;
        do
        {
            Console.WriteLine(prompt);
            input = Console.ReadLine()?.Trim();
            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Book title cannot be empty. Please try again.");
            }
        } while (string.IsNullOrWhiteSpace(input));

        return input!;
    }

    // Helper: find a book in a collection by title (case-insensitive)
    private static Book? FindBookByTitle(string searchTitle, IEnumerable<Book> listOfBooks)
        => listOfBooks.FirstOrDefault(book => string.Equals(book.Title, searchTitle, StringComparison.OrdinalIgnoreCase)); 
}

// Book class representing a book in the library
public class Book
{
    public string Title { get; set; }

    public bool IsBorrowed { get; private set; }

    // Constructor to initialize a book with a title
    public Book(string title)
    {
        Title = title;
        IsBorrowed = false;
    }

    // Get the status of the book depending wether it's borrowed or in the library
    public string GetBookStatus()
    {
        return $"{(IsBorrowed ? "(Checked-Out)" : "")}";
    }

    // Mark the book as borrowed
    public void BorrowBook()
    {
        IsBorrowed = true;
    }

    // Mark the book as returned
    public void ReturnBook()
    {
        IsBorrowed = false;
    }
}