using SharedModels;

namespace Sudoku.Http
{
    public interface ISudokuClient
    {
        public Task<SudokuBoardViewModel?> GetRandomSudokuAsync();
        public Task<SudokuBoardViewModel?> GetSudokuBySeedAsync(int seed);
        public Task<SudokuBoardViewModel?> GetSudokuByLevelAsync(int level);
        public Task<int[,]?> PostSolveSudokuAsync(int[,] board);
    }
}
