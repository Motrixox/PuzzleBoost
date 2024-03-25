namespace SudokuWebService.ViewModels
{
    public class SudokuBoardViewModel
    {
        public int Id { get; set; }
        public int[][] Board { get; set; }
        public int[][] Solution { get; set; }
        public float Difficulty { get; set; }
    }
}
