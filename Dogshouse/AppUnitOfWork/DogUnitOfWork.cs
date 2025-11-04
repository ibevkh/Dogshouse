using Dogshouse.AppUnitOfWork.Contracts;
using Dogshouse.Context;
using Dogshouse.Repositories;
using Dogshouse.Repositories.Contracts;
using URF.Core.EF;

namespace Dogshouse.AppUnitOfWork;

public class DogUnitOfWork : UnitOfWork, IDogUnitOfWork
{
    private readonly DogContext _context;
    private IDogRepository _dogs;

    public DogUnitOfWork(DogContext context) : base(context)
    {
        _context = context;
    }

    public virtual IDogRepository Dogs => _dogs ??= new DogRepository(_context);

    public void Dispose()
    {
        _context.Dispose();
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}
