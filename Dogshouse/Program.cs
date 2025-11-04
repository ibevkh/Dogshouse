
using Dogshouse.AppUnitOfWork;
using Dogshouse.Context;
using Dogshouse.Extensions;
using Dogshouse.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Dogshouse
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            // ---------- DATABASE ----------
            builder.Services.AddDbContext<DogContext>(opt =>
            {
                opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionString"));
            });

            // ---------- DATABASE SEEDER ----------
            builder.Services.AddScoped<DbSeeder>();

            //----------- REPOSITPRY + UNIT OF WORK -------------
            builder.Services.AddScoped<DogRepository>();
            builder.Services.AddScoped<DogUnitOfWork>();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            await app.SeedDatabaseAsync();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
