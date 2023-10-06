using Microsoft.EntityFrameworkCore;
using ProEventos.Domain.Identity;
using ProEventos.Domain.Interfaces.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProEventos.Persistence.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        protected UserRepository(DbContext context) : base(context) {}

        public async Task<IEnumerable<User>> GetAllUsersAsync()
            => await _context.Set<User>().ToListAsync();

        public async Task<User> GetUserByIdAsync(int id)
            => await _context.Set<User>()
                .FirstOrDefaultAsync(u => u.Id == id);

        public async Task<User> GetUserByNameAsync(string name)
            => await _context.Set<User>()
                .FirstOrDefaultAsync(u => u.PrimeiroNome == name);
    }
}
