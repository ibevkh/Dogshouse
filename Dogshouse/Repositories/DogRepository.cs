using Dogshouse.Context;
using Dogshouse.Entities;
using Dogshouse.Models;
using Dogshouse.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using URF.Core.EF;

namespace Dogshouse.Repositories;

public class DogRepository : Repository<Dog>, IDogRepository
{
    public DogRepository(DogContext context) : base(context)
    {
    }
    public async virtual Task<IEnumerable<Dog>> GetAllAsync(QueryParameters parameters)
    {
        IQueryable<Dog> query = Queryable();

        if (!string.IsNullOrEmpty(parameters.Attribute))
        {
            bool desc = parameters.Order?.ToLower() == "desc";
            query = parameters.Attribute.ToLower() switch
            {
                "name" => desc ? query.OrderByDescending(d => d.Name) : query.OrderBy(d => d.Name),
                "color" => desc ? query.OrderByDescending(d => d.Color) : query.OrderBy(d => d.Color),
                "tail_length" => desc ? query.OrderByDescending(d => d.TailLength) : query.OrderBy(d => d.TailLength),
                "weight" => desc ? query.OrderByDescending(d => d.Weight) : query.OrderBy(d => d.Weight),
                _ => query
            };
        }

        int pageNumber = parameters.PageNumber <= 0 ? 1 : parameters.PageNumber;
        int pageSize = parameters.PageSize <= 0 ? 10 : parameters.PageSize;

        query = query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);

        return await query.ToListAsync();
    }

    public async virtual Task<bool> ExistsByNameAsync(string name)
    {
        return await Queryable()
             .AnyAsync(d => d.Name.ToLower() == name.ToLower());
    }
}
