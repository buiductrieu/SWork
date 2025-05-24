using SWork.Common;
using System.Linq.Expressions;


namespace SWork.RepositoryContract.Basic
{
    public interface IGenericRepository<T>
    {
        IQueryable<T> GetAll();
        Task<T?> GetByIdAsync(int id);
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>> filter, string? includeProperties);
        Task InsertAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);
        void Update(T entity);
        void Delete(T entity);

        void DeleteRange(List<T> entities);
        Task<T> GetFirstOrDefaultAsync(
            Expression<Func<T, bool>> predicate,
            string? includeProperties = null
        );
        Task<Pagination<T>> GetPaginationAsync(
            Expression<Func<T, bool>>? predicate = null,
            string? includeProperties = null,
            int pageIndex = 0,
            int pageSize = 10,
            Expression<Func<T, object>>? orderBy = null,
            bool isDescending = false
            );


    }
}
