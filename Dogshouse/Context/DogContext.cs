using Dogshouse.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Dogshouse.Context;

public class DogContext : DbContext
{
    public DogContext(DbContextOptions<DogContext> options) : base(options)
    {
        Console.WriteLine($"[DEBUG] DogContext initialized with connection: {this.Database.GetConnectionString()}");
    }

    public DbSet<Dog> Dogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DogConfiguration).Assembly);
    }
}
