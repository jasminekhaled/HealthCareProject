using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Core.IRepositories
{
    public interface IBaseRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task<T> AddAsync(T entity);
        Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
        void Update(T entity);
        void UpdateRange(IEnumerable<T> entities);
        Task<bool> AnyAsync(Expression<Func<T, bool>> check);
        Task<bool> FilterAndAnyAsync(Expression<Func<T, bool>> check, Expression<Func<T, bool>> filter = null);
        Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> criteria);
        Task<int> CountAsync(Expression<Func<T, bool>> criteria);
        Task<IEnumerable<T>> Where(Expression<Func<T, bool>> criteria);
        Task<IEnumerable<TResult>> Select<TResult>(Expression<Func<T, TResult>> criteria);
        Task<IEnumerable<TResult>> GetSpecificItems<TResult>(Expression<Func<T, bool>> filter, Expression<Func<T, TResult>> select);
        Task<T> GetSpecificItem(Expression<Func<T, bool>> filter, Expression<Func<T, bool>> single);
        Task<T> GetFirstItem(Expression<Func<T, bool>> filter);
     //   Task<IEnumerable<T>> OrderByDescendingTheTop5(Expression<Func<T, object>> orderBy, int? take);
        Task<IEnumerable<TResult>> GetSelectedAsync<TKey, TResult>(
            Expression<Func<T, bool>> filter,
            Expression<Func<T, TKey>> groupBy,
            Expression<Func<IGrouping<TKey, T>, TResult>> select,
            Expression<Func<TResult, object>> orderBy,
            int take);

        Task<TResult> GetOneAsync<TResult>(
            Expression<Func<T, bool>> filter,
            Expression<Func<T, TResult>> select,
            Expression<Func<TResult, object>> orderBy);

        Task<TResult> GetMultiSelectAsync<TKey, TResult>(
            Expression<Func<T, bool>> filter,
            Expression<Func<T, TResult>> firstSelect,
            Expression<Func<TResult, TKey>> groupBy,
            Expression<Func<IGrouping<TKey, TResult>, TResult>> secondSelect,
            Expression<Func<TResult, object>> orderBy,
            Expression<Func<TResult, TResult>> thirdSelect);
    }
}
