
using Dogshouse.AppUnitOfWork;
using Dogshouse.Context;
using Dogshouse.Extensions;
using Dogshouse.Mapping;
using Dogshouse.Middleware;
using Dogshouse.Repositories;
using Dogshouse.Services;
using Dogshouse.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Threading.RateLimiting;

namespace Dogshouse
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ---------- DATABASE ----------
            builder.Services.AddDbContext<DogContext>(opt =>
            {
                opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionString"));
            });

            // ---------- AUTOMAPPER ----------
            builder.Services.AddAutoMapper(typeof(DogMapping));

            // ---------- DATABASE SEEDER ----------
            builder.Services.AddScoped<DbSeeder>();

            //--- REPOSITPRY + UNIT OF WORK + SERVICES ---
            builder.Services.AddScoped<DogRepository>();
            builder.Services.AddScoped<DogUnitOfWork>();
            builder.Services.AddScoped<IDogService, DogService>();

            builder.Services.AddControllers();

            // ---------- CORS ----------
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    policy =>
                    {
                        policy.AllowAnyOrigin()
                              .AllowAnyHeader()
                              .AllowAnyMethod();
                    });
            });

            // ---------- RATE LIMITING ----------
            builder.Services.AddRateLimiter(options =>
            {
                options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
                {
                    var ip = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

                    return RateLimitPartition.GetFixedWindowLimiter(ip, _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 10, // 10 запитів
                        Window = TimeSpan.FromSeconds(1), // на секунду
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        QueueLimit = 0
                    });
                });

                options.RejectionStatusCode = 429;
            });

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

            // ---------- MIDDLEWARE ----------
            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.UseRateLimiter();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
