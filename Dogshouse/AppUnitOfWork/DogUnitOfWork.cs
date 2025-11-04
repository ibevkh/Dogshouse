using Dogshouse.Context;
using Dogshouse.Repositories;
using URF.Core.EF;

namespace Dogshouse.AppUnitOfWork;

public class DogUnitOfWork : UnitOfWork
{
    private readonly DogContext _context;
    private DogRepository _dogs;

    public DogUnitOfWork(DogContext context) : base(context)
    {
        _context = context;
    }

    public DogRepository Dogs => _dogs ??= new DogRepository(_context);

    public void Dispose()
    {
        _context.Dispose();
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}
