using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProEventos.Domain.Models;

namespace ProEventos.Domain.Interfaces.Repositories
{
    public interface IEventoRepository : IRepository<Evento>
    {
        Task<Evento> GetEventoByIdAsync(int id, bool includeEventos = false);
        Task<IEnumerable<Evento>> GetAllEventosAsync(bool includeEventos = false);
        Task<IEnumerable<Evento>> GetAllEventosByTemaAsync(string tema, bool includeEventos = false);
    }
}