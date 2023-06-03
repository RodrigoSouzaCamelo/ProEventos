using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProEventos.Domain.Models;

namespace ProEventos.Application.Interfaces
{
    public interface IEventoService
    {
        Task<Evento> Add(Evento evento);
        Task<Evento> Update(Evento evento);
        Task<bool> Delete(Evento evento);
        Task<bool> DeleteRange(Evento[] entities);
        Task<Evento> GetEventoByIdAsync(int id, bool includeEventos = false);
        Task<IEnumerable<Evento>> GetAllEventosAsync(bool includeEventos = false);
        Task<IEnumerable<Evento>> GetAllEventosByTemaAsync(string tema, bool includeEventos = false);
    }
}