using System.ComponentModel.DataAnnotations;

namespace SudokuWebService.Models
{
    public class Sudoku : IEntity
    {
        public int Id { get; set; }
        [MaxLength(81)]
        public required string Puzzle { get; set; }
        [MaxLength(81)]
        public required string Solution { get; set; }
        public int Clues { get; set; }
        public float Difficulty { get; set; }
    }
}
