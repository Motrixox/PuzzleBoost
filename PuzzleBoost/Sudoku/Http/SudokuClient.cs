using SharedModels;

namespace Sudoku.Http
{
    public class SudokuClient : ISudokuClient
    {
        private readonly HttpClient _httpClient;

        public SudokuClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<SudokuBoardViewModel?> GetRandomSudokuAsync()
        {
            var response = await _httpClient.GetAsync("GetRandomSudoku");

            return await HandleSudokuResponse(response);
        }

        public async Task<SudokuBoardViewModel?> GetSudokuBySeedAsync(int seed)
        {
            var response = await _httpClient.GetAsync("GetSudokuBySeed/" + seed);

            return await HandleSudokuResponse(response);
        }

        public async Task<SudokuBoardViewModel?> GetSudokuByLevelAsync(int level)
        {
            var response = await _httpClient.GetAsync("GetSudokuByLevel/" + level);

            return await HandleSudokuResponse(response);
        }

        public async Task<int[,]?> PostSolveSudokuAsync(int[,] board)
        {
            var response = await _httpClient.PostAsJsonAsync("SolveSudoku/", board);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var content = await response.Content.ReadFromJsonAsync<int[,]>();
            return content;
        }

        private async Task<SudokuBoardViewModel?> HandleSudokuResponse(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                var model = new SudokuBoardViewModel
                {
                    Id = 0,
                    Difficulty = 0
                };
                
                return model;
            }

            var content = await response.Content.ReadFromJsonAsync<SudokuBoardViewModel>();
            return content;
        }
    }
}
