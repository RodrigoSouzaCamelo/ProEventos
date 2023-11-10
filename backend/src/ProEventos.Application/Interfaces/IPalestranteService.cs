using ProEventos.Application.DTOs;
using ProEventos.Domain.Models;
using System.Threading.Tasks;

namespace ProEventos.Application.Interfaces
{
    public interface IPalestranteService
    {
        Task<PalestranteDTO> Add(int userId, PalestranteAddDTO palestranteDTO);
        Task<PalestranteDTO> Update(int userId, PalestranteUpdateDTO palestranteDTO);
        Task<PalestranteDTO> GetPalestranteByUserIdAsync(int userId, bool includeEventos = false);
        Task<PageList<PalestranteDTO>> GetAllPalestrantesAsync(PageParams pageParams, bool includeEventos = false);
    }
}
