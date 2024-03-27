using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SudokuWebService.Models;
using SudokuWebService.Services;

namespace SudokuWebService.Data
{
    public class MongoDbContext<T> where T : class, IEntity
    {
        private MongoClient _client;
        private IMongoDatabase _database;
        public IMongoCollection<T> _collection;

        public MongoDbContext(IOptions<DatabaseSettings> databaseSettings)
        {
            _client = new MongoClient(databaseSettings.Value.ConnectionString);

            _database = _client.GetDatabase("Sudoku");

            _collection = _database.GetCollection<T>("Sudoku");
        }

    }
}
