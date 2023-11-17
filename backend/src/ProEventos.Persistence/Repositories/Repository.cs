using Microsoft.EntityFrameworkCore;
using ProEventos.Domain.Interfaces.Repositories;
using System.Threading.Tasks;

namespace ProEventos.Persistence.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly DbContext _context;

        protected Repository(DbContext context)
        {
            _context = context;
        }

        public void Add(T entity)
            => _context.Set<T>().Add(entity);


        public void Update(T entity)
            => _context.Set<T>().Update(entity);

        public void Delete(T entity)
            => _context.Set<T>().Remove(entity);

        public void DeleteRange(T[] entities)
            => _context.Set<T>().RemoveRange(entities);

        public async Task<bool> SaveChangesAsync()
            => (await _context.SaveChangesAsync()) > 0;
    }
}