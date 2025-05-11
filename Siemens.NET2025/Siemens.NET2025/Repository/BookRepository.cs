using System.Text.Json;
using Siemens.NET2025.Model;

namespace Siemens.NET2025.Repository;

public class BookRepository
{
    private readonly string filePath = "Data/books.json";
    private List<Book> books;

    public BookRepository()
    {
        if (!Directory.Exists("Data"))
            Directory.CreateDirectory("Data");

        if (File.Exists(filePath))
        {
            var json = File.ReadAllText(filePath);
            books = JsonSerializer.Deserialize<List<Book>>(json) ?? new List<Book>();
        }
        else
        {
            books = new List<Book>();
            SaveChanges();
        }
    }

    private void SaveChanges()
    {
        var json = JsonSerializer.Serialize(books, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(filePath, json);
    }

    public List<Book> GetAll() => books;

    public Book? GetById(long id) => books.FirstOrDefault(b => b.Id == id);

    public void Add(Book book)
    {
        Console.WriteLine($"Existing books: {books.Count}");

        var existingIds = books.Select(b => b.Id).OrderBy(id => id).ToList();
        long nextId = 0;

        foreach (var id in existingIds)
        {
            if (id != nextId)
                break;
            nextId++;
        }

        Console.WriteLine($"Assigned ID: {nextId}");

        book.Id = nextId;
        books.Add(book);
        SaveChanges();
    }

    public void Update(long id,Book book)
    {
        var index = books.FindIndex(b => b.Id == id);
        if (index >= 0)
        {
            books[index] = book;
            SaveChanges();
        }
    }

    public void Delete(long id)
    {
        books.RemoveAll(b => b.Id == id);
        SaveChanges();
    }

    public bool SetAvailableQuantity(long id, long quantity)
    {
        var index = books.FindIndex(b => b.Id == id);
        if (index < 0)
        {
            return false;
        }

        books[index].AvailableQuantity = quantity;
        SaveChanges();
        return true;
    }

    public List<Book> FilterByAuthor(string author)
    {
        return books.Where(b => b.Author.Equals(author)).ToList();
    }
    
    public List<Book> FilterByTitle(string title)
    {
        return books.Where(b => b.Title.Equals(title)).ToList();
    }

    public List<Book> FilterByCategory(Category category)
    {
        return books.Where(b => b.Categories.Contains(category)).ToList();
    }
}