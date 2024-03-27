using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace SudokuWebService.Models
{
    public class Sudoku : IEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? _id { get; set; }
        public int id { get; set; }
        [MaxLength(81)]
        public required string puzzle { get; set; }
        [MaxLength(81)]
        public required string solution { get; set; }
        public int clues { get; set; }
        public double difficulty { get; set; }
    }
}
