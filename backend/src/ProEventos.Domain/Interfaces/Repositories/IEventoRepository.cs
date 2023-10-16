using ProEventos.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProEventos.Domain.Interfaces.Repositories
{
    public interface IEventoRepository : IRepository<Evento>
    {
        Task<Evento> GetEventoByIdAsync(int userId, int id, bool includeEventos = false);
        Task<IEnumerable<Evento>> GetAllEventosAsync(int userId, bool includeEventos = false);
        Task<IEnumerable<Evento>> GetAllEventosByTemaAsync(int userId, string tema, bool includeEventos = false);
    }
}