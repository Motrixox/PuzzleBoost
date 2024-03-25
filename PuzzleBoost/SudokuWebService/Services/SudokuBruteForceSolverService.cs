using SudokuWebService.Extensions;
using System.Text;

namespace SudokuWebService.Services
{
    public class SudokuBruteForceSolverService : ISudokuSolverService
    {        
        public bool SolveSudoku(int[,] board, out int[,] boardSolved)
        {
            boardSolved = new int[9, 9];
            Array.Copy(board, boardSolved, 81);

            return SolveSudokuRecursive(boardSolved);
        }
        public int CountSolutions(int[,] board)
        {
            int[,] boardSolved = new int[9, 9];
            Array.Copy(board, boardSolved, 81);

            int solutions = 0;
            CountSolutionsRecursive(boardSolved, ref solutions);

            return solutions;
        }

        private bool SolveSudokuRecursive(int[,] board)
        {
            int row, col;

            if (!FindEmptyLocation(out row, out col, board))
            {
                return true;
            }

            for (int num = 1; num <= 9; num++)
            {
                if (IsSafe(row, col, num, board))
                {
                    board[row, col] = num;

                    if (SolveSudokuRecursive(board))
                    {
                        return true;
                    }

                    board[row, col] = 0;
                }
            }

            return false;
        }

        private bool CountSolutionsRecursive(int[,] board, ref int solutions)
        {
            if (solutions > 1)
                return false;

            int row, col;

            if (!FindEmptyLocation(out row, out col, board))
            {
                solutions++;
                return false;
            }

            for (int num = 1; num <= 9; num++)
            {
                if (IsSafe(row, col, num, board))
                {
                    board[row, col] = num;

                    if (CountSolutionsRecursive(board, ref solutions))
                    {
                        return true;
                    }

                    board[row, col] = 0;
                }
            }

            return false;
        }

        private bool FindEmptyLocation(out int row, out int col, int[,] board)
        {
            for (row = 0; row < 9; row++)
            {
                for (col = 0; col < 9; col++)
                {
                    if (board[row, col] == 0)
                    {
                        return true;
                    }
                }
            }
            row = -1;
            col = -1;
            return false;
        }

        private bool IsSafe(int row, int col, int num, int[,] board)
        {
            return !UsedInRow(row, num, board) && !UsedInCol(col, num, board) && !UsedInBox(row - row % 3, col - col % 3, num, board);
        }

        private bool UsedInRow(int row, int num, int[,] board)
        {
            for (int col = 0; col < 9; col++)
            {
                if (board[row, col] == num)
                {
                    return true;
                }
            }
            return false;
        }

        private bool UsedInCol(int col, int num, int[,] board)
        {
            for (int row = 0; row < 9; row++)
            {
                if (board[row, col] == num)
                {
                    return true;
                }
            }
            return false;
        }

        private bool UsedInBox(int boxStartRow, int boxStartCol, int num, int[,] board)
        {
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    if (board[row + boxStartRow, col + boxStartCol] == num)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
