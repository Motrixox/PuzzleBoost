using Microsoft.EntityFrameworkCore;
using SudokuWebService.Models;

namespace SudokuWebService.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Sudoku> Sudoku { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
