using Microsoft.EntityFrameworkCore;
using SudokuWebService.Data;
using SudokuWebService.Extensions;
using SudokuWebService.Models;
using System.Linq.Expressions;
using System.Security.Principal;

namespace SudokuWebService.Services
{
    public class RepositoryService<T> : IRepositoryService<T> where T : class, IEntity
    {
        protected ApplicationDbContext _context;
        protected DbSet<T> _set;

        public RepositoryService(ApplicationDbContext context)
        {
            _context = context;
            _set = _context.Set<T>();
        }

        public async Task<ServiceResult> AddAsync(T entity)
        {
            ServiceResult result = new ServiceResult();
            try
            {
                _set.Add(entity);
                await SaveAsync();
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
                _set.Remove(entity);
                await SaveAsync();
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
                _context.Entry(entity).State = EntityState.Modified;
                await SaveAsync();
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
            return await _set.FirstOrDefaultAsync(r => r.Id == id);
        }

        public IQueryable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            IQueryable<T> query = _set.Where(predicate);
            return query;
        }

        public IQueryable<T> GetAllRecords()
        {
            return _set;
        }

        private async Task<ServiceResult> SaveAsync()
        {
            ServiceResult result = new ServiceResult();
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                result.Result = ServiceResultStatus.Error;
                result.Messages.Add(e.Message);
            }

            return result;
        }
    }

}
