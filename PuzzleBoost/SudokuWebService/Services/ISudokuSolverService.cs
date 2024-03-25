namespace SudokuWebService.Services
{
    public interface ISudokuSolverService
    {
        public bool SolveSudoku(int[,] board, out int[,] boardSolved);
    }
}
