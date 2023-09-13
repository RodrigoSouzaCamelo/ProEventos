using ProEventos.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProEventos.Application.Interfaces
{
    public interface ILoteService
    {
        Task<IEnumerable<LoteDTO>> SaveLotes(int eventoId, IEnumerable<LoteDTO> models);
        Task<bool> DeleteLote(int eventoId, int loteId);

        Task<IEnumerable<LoteDTO>> GetLotesByEventoIdAsync(int eventoId);
        Task<LoteDTO> GetLoteByIdsAsync(int eventoId, int loteId);
    }
}
