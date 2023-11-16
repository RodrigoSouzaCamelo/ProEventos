using ProEventos.Application.DTOs;
using System.Threading.Tasks;

namespace ProEventos.Application.Interfaces
{
    public interface IRedeSocialService
    {
        Task<RedeSocialDTO[]> SaveByEvento(int eventoId, RedeSocialDTO[] dtos);
        Task<bool> DeleteByEvento(int eventoId, int redeSocialId);

        Task<RedeSocialDTO[]> SaveByPalestrante(int palestranteId, RedeSocialDTO[] dtos);
        Task<bool> DeleteByPalestrante(int palestranteId, int redeSocialId);

        Task<RedeSocialDTO[]> GetAllByEventoIdAsync(int eventoId);
        Task<RedeSocialDTO[]> GetAllByPalestranteIdAsync(int palestranteId);

        Task<RedeSocialDTO> GetByEventoIdAsync(int eventoId, int redeSocialId);
        Task<RedeSocialDTO> GetByPalestranteIdAsync(int palestranteId, int redeSocialId);
    }
}
