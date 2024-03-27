using SudokuWebService.Services;
using SudokuWebService.Data;
using SudokuWebService.Models;
using Microsoft.EntityFrameworkCore;

namespace SudokuWebService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            //builder.Services.AddDbContext<ApplicationDbContext>(options =>
            //    options.UseSqlServer(connectionString));

            // Add services to the container.
            builder.Services.AddControllers();

            builder.Services.AddScoped<ISudokuConverter, SudokuConverter>();
            builder.Services.AddScoped<ISudokuSolverService, SudokuBruteForceSolverService>();
            builder.Services.AddScoped<IRepositoryService<Sudoku>, RepositoryService<Sudoku>>();

            builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("MongoConnectionString"));

            builder.Services.AddSingleton<MongoDbContext<Sudoku>>();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                builder =>
                {
                    builder.WithOrigins("http://localhost:4200")
                                        .AllowAnyHeader()
                                        .AllowAnyMethod()
                                        .AllowCredentials();
                });
            });


            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseAuthorization();

            app.UseCors("CorsPolicy");

            app.MapControllers();

            app.Run();
        }
    }
}
