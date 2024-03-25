using Microsoft.AspNetCore.Mvc;
using SharedModels;
using Sudoku.Http;

namespace Sudoku.Controllers
{
    public class SudokuController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ISudokuClient _communicationService;

        public SudokuController(ILogger<HomeController> logger,
            ISudokuClient communication)
        {
            _logger = logger;
            _communicationService = communication;
        }

        public IActionResult Index(SudokuBoardViewModel model)
        {
            return View(model);
        }

        [Route("sudoku/")]
        public async Task<IActionResult> RandomSudoku()
        {
            try
            {
                var model = await _communicationService.GetRandomSudokuAsync();

                return View(nameof(Index), model);
            }
            catch (Exception e) 
            {
                _logger.LogError(e.Message);
            }

            return View(nameof(Index), new SudokuBoardViewModel());
        }

        [Route("sudoku/seed/{seed}")]
        public async Task<IActionResult> SudokuBySeed(int seed)
        {
            try
            {
                var model = await _communicationService.GetSudokuBySeedAsync(seed);

                return View(nameof(Index), model);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }

            return View(nameof(Index), new SudokuBoardViewModel());
        }

        [Route("sudoku/level/{level}")]
        public async Task<IActionResult> SudokuByLevel(int level)
        {
            try
            {
                var model = await _communicationService.GetSudokuByLevelAsync(level);

                return View(nameof(Index), model);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }

            return View(nameof(Index), new SudokuBoardViewModel());
        }
    }
}
