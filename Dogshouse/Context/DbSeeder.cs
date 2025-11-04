using Dogshouse.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dogshouse.Context;

public class DbSeeder
{
    private readonly DogContext _context;
    private readonly ILogger<DbSeeder> _logger;

    public DbSeeder(DogContext context, ILogger<DbSeeder> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task SeedAsync()
    {
        if (!await _context.Dogs.AnyAsync())
        {
            _logger.LogInformation("Seeding initial dogs...");

            var dogs = new List<Dog>
                {
                    new Dog { Name = "Neo", Color = "red&amber", TailLength = 22, Weight = 32 },
                    new Dog { Name = "Jessy", Color = "black&white", TailLength = 7, Weight = 14 }
                };

            _context.Dogs.AddRange(dogs);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Database seeding completed.");
        }
        else
        {
            _logger.LogInformation("Dogs table already contains data. Skipping seed.");
        }
    }
}
