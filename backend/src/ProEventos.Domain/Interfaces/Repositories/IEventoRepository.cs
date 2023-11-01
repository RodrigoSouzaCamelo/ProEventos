using ProEventos.Domain.Models;
using System.Threading.Tasks;

namespace ProEventos.Domain.Interfaces.Repositories
{
    public interface IEventoRepository : IRepository<Evento>
    {
        Task<Evento> GetEventoByIdAsync(int userId, int id, bool includeEventos = false);
        Task<PageList<Evento>> GetAllEventosAsync(int userId, PageParams pageParams, bool includeEventos = false);
    }
}