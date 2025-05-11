namespace Siemens.NET2025.Model;

public class Lending
{
    public long Id {get; set;}
    public long UserId {get; set;}
    public long BookId {get; set;}
    public DateTime DateBorrowed {get; set;}
    public DateTime DateReturned {get; set;}
}