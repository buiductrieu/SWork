using SWork.Common;
using SWork.RepositoryContract.Basic;
using System.Linq.Expressions;

namespace SWork.Repository.Basic
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly SWorkDbContext _context;
        internal DbSet<T> dbSet;

        public GenericRepository(SWorkDbContext context)
        {
            _context = context;
            dbSet = _context.Set<T>();
        }

        public IQueryable<T> GetAll()
        {
            return _context.Set<T>().AsQueryable();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<List<T>> GetAllAsync(
            Expression<Func<T, bool>> filter,
            string? includeProperties = null
        )
        {
            IQueryable<T> query = dbSet;
            query = query.Where(filter);
            if (includeProperties != null)
            {
                foreach (
                    var item in includeProperties.Split(
                        new char[] { ',' },
                        StringSplitOptions.RemoveEmptyEntries
                    )
                )
                {
                    query = query.Include(item);
                }
            }
            return await query.ToListAsync();
        }

        public async Task InsertAsync(T entity)
        {
            await dbSet.AddAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await dbSet.AddRangeAsync(entities);
        }

        public void Update(T entity)
        {
            dbSet.Update(entity);
        }

        public void Delete(T entity)
        {
            dbSet.Remove(entity);
        }

        public void DeleteRange(List<T> entities)
        {
            dbSet.RemoveRange(entities);
        }

        public async Task<T> GetFirstOrDefaultAsync(
            Expression<Func<T, bool>> predicate,
            string? includeProperties = null
        )
        {
            IQueryable<T> query = dbSet;
            query = query.Where(predicate);
            if (includeProperties != null)
            {
                foreach (
                    var item in includeProperties.Split(
                        new char[] { ',' },
                        StringSplitOptions.RemoveEmptyEntries
                    )
                )
                {
                    query = query.Include(item);
                }
            }
            return await query.FirstOrDefaultAsync();
        }

        public async Task<Pagination<T>> GetPaginationAsync(
            Expression<Func<T, bool>>? predicate = null,
            string? includeProperties = null,
            int pageIndex = 0,
            int pageSize = 10,
            Expression<Func<T, object>>? orderBy = null,
            bool isDescending = false
        )
        {
            IQueryable<T> query = dbSet;
            if (predicate != null)
                query = query.Where(predicate);
            if (includeProperties != null)
            {
                foreach (
                    var item in includeProperties.Split(
                        new char[] { ',' },
                        StringSplitOptions.RemoveEmptyEntries
                    )
                )
                {
                    query = query.Include(item);
                }
            }
            if (orderBy != null)
            {
                query = isDescending ? query.OrderByDescending(orderBy) : query.OrderBy(orderBy);
            }
            var itemCount = await query.CountAsync();
            var items = await query
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();
            var result = new Pagination<T>()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalItemsCount = itemCount,
                Items = items,
            };

            return result;
        }
    }
}
