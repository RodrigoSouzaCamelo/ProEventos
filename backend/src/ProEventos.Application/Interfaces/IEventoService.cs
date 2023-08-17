using ProEventos.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProEventos.Application.Interfaces
{
    public interface IEventoService
    {
        Task<EventoDTO> Add(EventoDTO evento);
        Task<EventoDTO> Update(EventoDTO evento);
        Task<bool> Delete(EventoDTO evento);
        Task<bool> DeleteRange(EventoDTO[] entities);
        Task<EventoDTO> GetEventoByIdAsync(int id, bool includeEventos = false);
        Task<IEnumerable<EventoDTO>> GetAllEventosAsync(bool includeEventos = false);
        Task<IEnumerable<EventoDTO>> GetAllEventosByTemaAsync(string tema, bool includeEventos = false);
    }
}