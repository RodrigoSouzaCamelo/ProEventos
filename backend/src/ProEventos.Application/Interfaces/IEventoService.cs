using ProEventos.Application.DTOs;
using ProEventos.Domain.Models;
using System.Threading.Tasks;

namespace ProEventos.Application.Interfaces
{
    public interface IEventoService
    {
        Task<EventoDTO> Add(int userId, EventoDTO evento);
        Task<EventoDTO> Update(int userId, EventoDTO evento);
        Task<bool> Delete(int userId, EventoDTO evento);
        Task<bool> DeleteRange(int userId, EventoDTO[] entities);
        Task<EventoDTO> GetEventoByIdAsync(int userId, int id, bool includeEventos = false);
        Task<PageList<EventoDTO>> GetAllEventosAsync(int userId, PageParams pageParams, bool includeEventos = false);
    }
}