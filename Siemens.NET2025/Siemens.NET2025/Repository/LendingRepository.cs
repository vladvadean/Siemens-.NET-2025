using System.Text.Json;
using Siemens.NET2025.Model;

namespace Siemens.NET2025.Repository;

public class LendingRepository
{
    private readonly string filePath = "Data/lendings.json";
    private List<Lending> lendings;

    public LendingRepository()
    {
        if (!Directory.Exists("Data"))
            Directory.CreateDirectory("Data");

        if (File.Exists(filePath))
        {
            var json = File.ReadAllText(filePath);
            lendings = JsonSerializer.Deserialize<List<Lending>>(json) ?? new List<Lending>();
        }
        else
        {
            lendings = new List<Lending>();
            SaveChanges();
        }
    }

    private void SaveChanges()
    {
        var json = JsonSerializer.Serialize(lendings, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(filePath, json);
    }

    public List<Lending> GetAll() => lendings;

    public Lending? GetById(long id) => lendings.FirstOrDefault(b => b.Id == id);

    public bool AvailableForBorrow(long bookId, long userId)
    {
        // a user can't borrow the same book more than 1 time simultaneously
        var results = lendings.Where(l => l.BookId == bookId && l.UserId == userId).ToList();
        if (results.Count != 0)
        {
            return false;
        }

        return true;
    }
    public void Add(long bookId, long userId)
    {
        Lending lending = new Lending();
        var existingIds = lendings.Select(b => b.Id).OrderBy(id => id).ToList();
        long nextId = 1;

        foreach (var id in existingIds)
        {
            if (id != nextId)
                break;
            nextId++;
        }

        lending.Id = nextId;
        lending.UserId = userId;
        lending.BookId = bookId;
        lending.DateBorrowed = DateTime.Now;

        lendings.Add(lending);
        SaveChanges();
    }

    public bool Return(long bookId, long userId)
    {
        // mark the first element from the list that has same id's and that does not have a return date
        var results = lendings.Where(l => l.BookId == bookId && l.UserId == userId && l.DateReturned == new DateTime()).ToList();
        if (results.Count == 0)
        {
            return false;
        }
        lendings[0].DateReturned = DateTime.Now;
        SaveChanges();
        return true;
    }

    public void Update(long id,Lending lending)
    {
        var index = lendings.FindIndex(b => b.Id == id);
        if (index >= 0)
        {
            lendings[index] = lending;
            SaveChanges();
        }
    }

    public void Delete(long id)
    {
        lendings.RemoveAll(b => b.Id == id);
        SaveChanges();
    }

    public List<Lending> FilterByDateBorrowed(DateTime date)
    {
        return lendings.Where(b => b.DateBorrowed.Equals(date)).ToList();
    }
    
    public List<Lending> FilterByDateReturned(DateTime date)
    {
        return lendings.Where(b => b.DateReturned.Equals(date)).ToList();
    }
    
    // get all lendings that have the borrowedDate bigger than a certain date
    public List<Lending> FilterUpToBorrowedDate(DateTime date)
    {
        return lendings.Where(b => b.DateReturned >= date).ToList();
    }
    
    // get all lendings that have the returnedDate bigger than a certain date
    public List<Lending> FilterUpToReturnedDate(DateTime date)
    {
        return lendings.Where(b => b.DateReturned >= date).ToList();
    }
    
    // get all lendings that were not yet returned
    public List<Lending> FilterByNotReturned(DateTime date)
    {
        return lendings.Where(b => b.DateReturned.Equals(new DateTime())).ToList();
    }
}