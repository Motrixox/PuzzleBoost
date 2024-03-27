using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
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
            var sudoku = await _sudokuRepository.GetSingleAsync(Random.Shared.Next(1, 1428000));

            if (sudoku == null)
            {
                return NotFound("Sudoku of random id not found");
            }

            var model = new SudokuBoardViewModel
            {
                Id = sudoku.id,
                Board = _sudokuConverter.ConvertStringToArray(sudoku.puzzle),
                Solution = _sudokuConverter.ConvertStringToArray(sudoku.solution),
                Difficulty = sudoku.difficulty
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
                Id = sudoku.id,
                Board = _sudokuConverter.ConvertStringToArray(sudoku.puzzle),
                Solution = _sudokuConverter.ConvertStringToArray(sudoku.solution),
                Difficulty = sudoku.difficulty
            };

            return Ok(model);
        }

        [Route("GetSudokuByLevel/{level}")]
        [HttpGet]
        public async Task<IActionResult> GetSudokuByLevel(int level)
        {
            var list = _sudokuRepository.GetAllRecords();

            IFindFluent<Sudoku, Sudoku> listFiltered;
            int skipMaxNumber; // number of sudoku boards of chosen level stored in database

            if (level == 1)
            {
                listFiltered = list.Find(x => x.difficulty == 0);
                skipMaxNumber = 615989; // arbitrary number set to improve performance
            }
            else if (level == 2)
            {
                listFiltered = list.Find(x => x.difficulty > 0 && x.difficulty <= 2.0);
                skipMaxNumber = 421680;
            }
            else if (level == 3)
            {
                listFiltered = list.Find(x => x.difficulty > 2.0 && x.difficulty <= 4.0);
                skipMaxNumber = 359204;
            }
            else if (level == 4)
            {
                listFiltered = list.Find(x => x.difficulty > 4.0 && x.difficulty <= 6.0);
                skipMaxNumber = 30634;
            }
            else if (level == 5)
            {
                listFiltered = list.Find(x => x.difficulty > 6.0);
                skipMaxNumber = 492;
            }
            else
            {
                return BadRequest("Invalid level number");
            }

            Sudoku ? sudoku = await listFiltered.Skip(Random.Shared.Next(0, skipMaxNumber)).FirstOrDefaultAsync();

            if (sudoku == null)
            {
                return NotFound("Sudoku of chosen difficulty not found");
            }

            var model = new SudokuBoardViewModel
            {
                Id = sudoku.id,
                Board = _sudokuConverter.ConvertStringToArray(sudoku.puzzle),
                Solution = _sudokuConverter.ConvertStringToArray(sudoku.solution),
                Difficulty = sudoku.difficulty
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
