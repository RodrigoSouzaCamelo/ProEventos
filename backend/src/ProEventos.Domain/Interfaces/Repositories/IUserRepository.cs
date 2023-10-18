using ProEventos.Domain.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProEventos.Domain.Interfaces.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(int id);
        Task<User> GetByUserName(string name);
    }
}
