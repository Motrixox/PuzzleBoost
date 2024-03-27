using MongoDB.Driver;
using SudokuWebService.Extensions;
using SudokuWebService.Models;
using System.Linq.Expressions;
using System.Security.Principal;

namespace SudokuWebService.Services
{
    public interface IRepositoryService<T> where T : IEntity
    {
        IMongoCollection<T> GetAllRecords();

        Task<T?> GetSingleAsync(int id);
        Task<T?> FindBy(Expression<Func<T, bool>> predicate);
        Task<ServiceResult> AddAsync(T entity);
        Task<ServiceResult> DeleteAsync(T entity);
        Task<ServiceResult> EditAsync(T entity);
        //Task<ServiceResult> SaveAsync();
    }
}
