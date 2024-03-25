using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SudokuWebService.Models;
using SudokuWebService.Services;
using SudokuWebService.ViewModels;

namespace SudokuWebService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SudokuController : ControllerBase
    {
        private readonly ILogger<SudokuController> _logger;
        private readonly ISudokuSolverService _sudokuSolver;
        private readonly IRepositoryService<Sudoku> _sudokuRepository;
        private readonly ISudokuConverter _sudokuConverter;

        public SudokuController(ILogger<SudokuController> logger,
            ISudokuSolverService solver,
            IRepositoryService<Sudoku> repository,
            ISudokuConverter converter)
        {
            _logger = logger;
            _sudokuSolver = solver;
            _sudokuRepository = repository;
            _sudokuConverter = converter;
        }

        [Route("GetRandomSudoku")]
        [HttpGet]
        public async Task<IActionResult> GetRandomSudoku()
        {
            //var sudoku = await _sudokuRepository.GetSingleAsync(Random.Shared.Next(3_000_000));
            var sudoku = await _sudokuRepository.GetSingleAsync(Random.Shared.Next(1,19894));

            if (sudoku == null)
            {
                return NotFound("Sudoku of random id not found");
            }

            var model = new SudokuBoardViewModel
            {
                Id = sudoku.Id,
                Board = _sudokuConverter.ConvertStringToArray(sudoku.Puzzle),
                Solution = _sudokuConverter.ConvertStringToArray(sudoku.Solution),
                Difficulty = sudoku.Difficulty
            };

            return Ok(model);
        }

        [Route("GetSudokuBySeed/{seed}")]
        [HttpGet]
        public async Task<IActionResult> GetSudokuBySeed(int seed)
        {
            var sudoku = await _sudokuRepository.GetSingleAsync(seed); 
            
            if (sudoku == null)
            {
                return NotFound("Sudoku of chosen seed not found");
            }

            var model = new SudokuBoardViewModel
            {
                Id = sudoku.Id,
                Board = _sudokuConverter.ConvertStringToArray(sudoku.Puzzle),
                Solution = _sudokuConverter.ConvertStringToArray(sudoku.Solution),
                Difficulty = sudoku.Difficulty
            };

            return Ok(model);
        }

        [Route("GetSudokuByLevel/{level}")]
        [HttpGet]
        public async Task<IActionResult> GetSudokuByLevel(int level)
        {
            var list = _sudokuRepository.GetAllRecords();

            //var random = Random.Shared.Next(0, list.Count()); // this is how it should look like in theory, but setting it manually to 3m saves 0,5s every request
            //var random = Random.Shared.Next(0, 3_000_000);
            var random = Random.Shared.Next(0, 19894);

            IQueryable<Sudoku> listFiltered;

            if (level == 1)
            {
                listFiltered = list.Where(x => x.Difficulty == 0);
            }
            else if (level == 2)
            {
                listFiltered = list.Where(x => x.Difficulty > 0 && x.Difficulty <= 2.0);
            }
            else if (level == 3)
            {
                listFiltered = list.Where(x => x.Difficulty > 2.0 && x.Difficulty <= 4.0);
            }
            else if (level == 4)
            {
                listFiltered = list.Where(x => x.Difficulty > 4.0 && x.Difficulty <= 6.0);
            }
            else if (level == 5)
            {
                listFiltered = list.Where(x => x.Difficulty > 6.0);
            }
            else
            {
                return BadRequest("Invalid level number");
            }

            Sudoku? sudoku = await listFiltered.OrderBy(x => Math.Abs(random - x.Id)).FirstOrDefaultAsync();

            if (sudoku == null)
            {
                return NotFound("Sudoku of chosen difficulty not found");
            }

            var model = new SudokuBoardViewModel
            {
                Id = sudoku.Id,
                Board = _sudokuConverter.ConvertStringToArray(sudoku.Puzzle),
                Solution = _sudokuConverter.ConvertStringToArray(sudoku.Solution),
                Difficulty = sudoku.Difficulty
            };

            return Ok(model);
        }

        [Route("SolveSudoku")]
        [HttpPost]
        public IActionResult SolveSudoku([FromBody]int[][] board)
        {
            var convertedBoard = _sudokuConverter.ConvertTo2DArray(board);
            if(_sudokuSolver.SolveSudoku(convertedBoard, out int[,] boardSolved))
            {
                var convertedBoardSolved = _sudokuConverter.ConvertToJaggedArray(boardSolved);
                return Ok(convertedBoardSolved);
            }
            return NotFound("Sudoku has no solutions.");
        }


    }
}
