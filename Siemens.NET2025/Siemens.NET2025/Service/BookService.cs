using Siemens.NET2025.Model;
using Siemens.NET2025.Repository;

namespace Siemens.NET2025.Service;

public class BookService
{
    private readonly BookRepository repository;

    public BookService(BookRepository repository)
    {
        this.repository = repository;
    }

    public List<Book> GetAll() => repository.GetAll();

    public Book? GetById(long id) => repository.GetById(id);

    public void Add(Book book) => repository.Add(book);

    public void Update(long id, Book book) => repository.Update(id, book);

    public void Delete(long id) => repository.Delete(id);

    public bool BorrowBook(long id)
    {
        var book = repository.GetById(id);
        if (book == null) return false;

        var quantity = book.AvailableQuantity;
        if (quantity <= 0) return false;

        book.AvailableQuantity--;
        repository.Update(id, book);
        return true;
    }

    public bool ReturnBook(long id)
    {
        var book = repository.GetById(id);
        if (book == null) return false;

        book.AvailableQuantity++;
        repository.Update(id, book);
        return true;
    }

    public List<Book> FilterByAuthor(string author)
    {
        return repository.FilterByAuthor(author);
    }

    public List<Book> FilterByTitle(string title)
    {
        return repository.FilterByTitle(title);
    }

    public List<Book> FilterByCategory(Category category)
    {
        return repository.FilterByCategory(category);
    }
}