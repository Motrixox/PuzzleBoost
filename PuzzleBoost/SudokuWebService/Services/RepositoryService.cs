using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using SudokuWebService.Data;
using SudokuWebService.Extensions;
using SudokuWebService.Models;
using System.Linq.Expressions;
using System.Security.Principal;

namespace SudokuWebService.Services
{
    public class RepositoryService<T> : IRepositoryService<T> where T : class, IEntity
    {
        protected MongoDbContext<T> _context;
        protected IMongoCollection<T> _collection;

        public RepositoryService(MongoDbContext<T> context)
        {
            _context = context;
            _collection = context._collection;
        }

        public async Task<ServiceResult> AddAsync(T entity)
        {
            ServiceResult result = new ServiceResult();
            try
            {
                await _collection.InsertOneAsync(entity);
            }
            catch (Exception e)
            {
                result.Result = ServiceResultStatus.Error;
                result.Messages.Add(e.Message);
            }

            return result;
        }

        public async Task<ServiceResult> DeleteAsync(T entity)
        {
            ServiceResult result = new ServiceResult();
            try
            {
                await _collection.DeleteOneAsync(x => x.id == entity.id);
            }
            catch (Exception e)
            {
                result.Result = ServiceResultStatus.Error;
                result.Messages.Add(e.Message);
            }

            return result;
        }

        public async Task<ServiceResult> EditAsync(T entity)
        {
            ServiceResult result = new ServiceResult();
            try
            {
                await _collection.ReplaceOneAsync(x => x.id == entity.id, entity);
            }
            catch (Exception e)
            {
                result.Result = ServiceResultStatus.Error;
                result.Messages.Add(e.Message);
            }

            return result;
        }

        public async Task<T?> GetSingleAsync(int id)
        {
            return await _collection.Find(x => x.id == id).FirstOrDefaultAsync();
        }

        public async Task<T?> FindBy(Expression<Func<T, bool>> predicate)
        {
            return await _collection.Find(predicate).FirstOrDefaultAsync();
        }

        public IMongoCollection<T> GetAllRecords()
        {
            return _collection;
        }
    }

}
