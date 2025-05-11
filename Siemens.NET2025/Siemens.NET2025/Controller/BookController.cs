using Microsoft.AspNetCore.Mvc;
using Siemens.NET2025.Model;
using Siemens.NET2025.Service;
using Microsoft.AspNetCore.Mvc;

namespace Siemens.NET2025.Controller;

[ApiController]
[Route("api/books")]
public class BooksController : ControllerBase
{
    private readonly BookService bookService;
    private readonly LendingService lendingService;

    public BooksController(BookService bookService, LendingService lendingService)
    {
        this.bookService = bookService;
        this.lendingService = lendingService;
    }

    [HttpGet("")]
    public ActionResult<List<Book>> GetAll()
    {
        return bookService.GetAll();
    }

    [HttpGet("{id}")]
    public ActionResult<Book> GetById(long id)
    {
        var book = bookService.GetById(id);
        if (book == null) return NotFound();
        return book;
    }

    [HttpPost("")]
    public IActionResult Add([FromBody] Book book)
    {
        bookService.Add(book);
        return CreatedAtAction(nameof(GetById), new { id = book.Id }, book);
    }

    [HttpPut("{id}")]
    public IActionResult Update(long id, [FromBody] Book updatedBook)
    {
        if (bookService.GetById(id) == null)
        {
            return NotFound();
        }

        bookService.Update(id, updatedBook);
        return Ok(updatedBook);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(long id)
    {
        bookService.Delete(id);
        return NoContent();
    }

    [HttpPost("{bookId}/{userId}/borrow")]
    public IActionResult Borrow(long bookId, long userId)
    {
        if (!lendingService.AvailableForBorrow(bookId, userId))
            return BadRequest("Book already borrowed");
        if (!bookService.BorrowBook(bookId))
            return BadRequest("Book not available");
        
        lendingService.CreateLending(bookId,userId);
        return Ok("Book borrowed");
    }

    [HttpPost("{bookId}/{userId}/return")]
    public IActionResult Return(long bookId, long userId)
    {
        if (!lendingService.ReturnLending(bookId, userId))
            return BadRequest("No book to be returned");
        if (!bookService.ReturnBook(bookId))
            return BadRequest("Invalid return");
        
        return Ok("Book returned");
        
    }

    [HttpGet("{author}/findAuthor")]
    public ActionResult<List<Book>> FilterByAuthor(string author)
    {
        return bookService.FilterByAuthor(author);
    }

    [HttpGet("{title}/findTitle")]
    public ActionResult<List<Book>> FilterByTitle(string title)
    {
        return bookService.FilterByTitle(title);
    }

    [HttpGet("{category}/findCategory")]
    public ActionResult<List<Book>> FilterByCategory(string category)
    {
        if (Enum.TryParse<Category>(category, true, out var parsedCategory))
        {
            return bookService.FilterByCategory(parsedCategory);
        }

        return BadRequest("Invalid category name.");
    }

    // return the most borrowed books up to date
    [HttpGet("{date}/bookLeaderboard")]
    public ActionResult<List<Book>> GetBookLeaderboard(DateTime date)
    {
        var lendings = lendingService.GetBorrowedAfter(date); // Use the LendingService
        var leaderboard = lendings
            .GroupBy(l => l.BookId)
            .OrderByDescending(g => g.Count())
            .Select(g => bookService.GetById(g.Key))
            .Where(b => b != null)
            .ToList();

        return Ok(leaderboard);
    }
    // return the id's of the users that borrowed the most books
    [HttpGet("/userLeaderboard")]
    public ActionResult<List<long>> GetUserLeaderboard([FromBody] DateTime date)
    {
        var lendings = lendingService.GetBorrowedAfter(date); // Use the LendingService
        var leaderboard = lendings
            .GroupBy(l => l.UserId)
            .OrderByDescending(g => g.Count())
            .Select(g => g.Key)
            .ToList();

        return Ok(leaderboard);
    }
    
    // return the most borrowed categories
    [HttpGet("categoryLeaderboard")]
    public ActionResult<List<string>> GetCategoryLeaderboard([FromBody] DateTime date)
    {
        
        var lendings = lendingService.GetBorrowedAfter(date);

        // get all books involved in those lendings
        var borrowedBooks = lendings
            .Select(l => bookService.GetById(l.BookId))
            .Where(b => b != null)
            .ToList();

        // flatten all categories and count frequency
        var categoryCounts = borrowedBooks
            .SelectMany(b => b.Categories)
            .GroupBy(c => c)
            .OrderByDescending(g => g.Count())
            .Select(g => $"{g.Key} ({g.Count()} times)")
            .ToList();

        return Ok(categoryCounts);
    }
    
    // get all the missing books
    [HttpGet("missing")]
    public ActionResult<List<object>> GetMissingBooks()
    {
        var notReturned = lendingService.GetNotReturned();

        var missingSummary = notReturned
            .GroupBy(l => l.BookId)
            .Select(g =>
            {
                var book = bookService.GetById(g.Key);
                return book == null ? null : new
                {
                    Title = book.Title,
                    BookId = book.Id,
                    MissingQuantity = g.Count()
                };
            })
            .Where(x => x != null)
            .ToList();

        return Ok(missingSummary);
    }

    
}