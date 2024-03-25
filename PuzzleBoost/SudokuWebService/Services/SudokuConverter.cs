namespace SudokuWebService.Services
{
    public class SudokuConverter : ISudokuConverter
    {
        public int[][] ConvertStringToArray(string input)
        {
            var result = new int[9][];

            for (int i = 0; i < 9; i++) 
            {
                result[i] = new int[9];
            }

            if (input.Length != 81)
                return result;

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++) 
                {
                    var c = input[i * 9 + j];

                    if (char.IsDigit(c))
                        result[i][j] = (int)char.GetNumericValue(c);
                    else
                        result[i][j] = 0;
                }
            }

            return result;
        }

        public int[][] ConvertToJaggedArray(int[,] array2D)
        {
            int rows = array2D.GetLength(0);
            int cols = array2D.GetLength(1);

            int[][] jaggedArray = new int[rows][];

            for (int i = 0; i < rows; i++)
            {
                jaggedArray[i] = new int[cols];
                for (int j = 0; j < cols; j++)
                {
                    jaggedArray[i][j] = array2D[i, j];
                }
            }

            return jaggedArray;
        }

        public int[,] ConvertTo2DArray(int[][] jaggedArray)
        {
            int rows = jaggedArray.Length;
            int cols = jaggedArray[0].Length;

            int[,] array2D = new int[rows, cols];

            for (int i = 0; i < rows; i++)
            {
                if (jaggedArray[i].Length != cols)
                {
                    throw new ArgumentException("Jagged array is not rectangular.");
                }

                for (int j = 0; j < cols; j++)
                {
                    array2D[i, j] = jaggedArray[i][j];
                }
            }

            return array2D;
        }
    }
}
