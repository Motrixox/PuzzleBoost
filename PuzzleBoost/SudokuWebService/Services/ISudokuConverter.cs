namespace SudokuWebService.Services
{
    public interface ISudokuConverter
    {
        public int[][] ConvertStringToArray(string input);
        public int[][] ConvertToJaggedArray(int[,] array2D);
        public int[,] ConvertTo2DArray(int[][] jaggedArray);
    }
}
