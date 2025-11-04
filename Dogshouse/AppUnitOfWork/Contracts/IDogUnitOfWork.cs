using Dogshouse.Repositories.Contracts;
using URF.Core.Abstractions;

namespace Dogshouse.AppUnitOfWork.Contracts;

public interface IDogUnitOfWork : IUnitOfWork, IDisposable
{
    IDogRepository Dogs { get; }
    Task<int> SaveChangesAsync();
}
