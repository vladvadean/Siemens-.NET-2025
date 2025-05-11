using Siemens.NET2025.Model;
using Siemens.NET2025.Repository;

namespace Siemens.NET2025.Service;

public class LendingService
{
    private readonly LendingRepository lendingRepository;

    public LendingService(LendingRepository lendingRepository)
    {
        this.lendingRepository = lendingRepository;
    }

    public void CreateLending(long bookId, long userId)
    {
        lendingRepository.Add(bookId, userId);
    }

    public bool AvailableForBorrow(long bookId, long userId)
    {
        return lendingRepository.AvailableForBorrow(bookId, userId);
    }

    public bool ReturnLending(long bookId, long userId)
    {
        return lendingRepository.Return(bookId, userId);
    }

    public Lending? GetLendingById(long id)
    {
        return lendingRepository.GetById(id);
    }

    public List<Lending> GetAll()
    {
        return lendingRepository.GetAll();
    }

    public void Update(long id, Lending lending)
    {
        lendingRepository.Update(id, lending);
    }

    public void Delete(long id)
    {
        lendingRepository.Delete(id);
    }

    public List<Lending> GetNotReturned()
    {
        return lendingRepository.FilterByNotReturned(DateTime.Now);
    }

    public List<Lending> GetBorrowedAfter(DateTime date)
    {
        return lendingRepository.FilterUpToBorrowedDate(date);
    }

    public List<Lending> GetReturnedAfter(DateTime date)
    {
        return lendingRepository.FilterUpToReturnedDate(date);
    }
}