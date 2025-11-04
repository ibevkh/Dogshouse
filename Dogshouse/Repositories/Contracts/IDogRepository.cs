using Dogshouse.Entities;
using Dogshouse.Models;
using URF.Core.Abstractions;

namespace Dogshouse.Repositories.Contracts;

public interface IDogRepository : IRepository<Dog>
{
    Task<IEnumerable<Dog>> GetAllAsync(QueryParameters parameters);
    Task<bool> ExistsByNameAsync(string name);
}
