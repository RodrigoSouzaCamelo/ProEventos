using ProEventos.Domain.Models;
using System.Threading.Tasks;

namespace ProEventos.Domain.Interfaces.Repositories
{
    public interface IPalestranteRepository : IRepository<Palestrante>
    {
        Task<Palestrante> GetPalestranteByUserIdAsync(int id, bool includeEventos = false);
        Task<PageList<Palestrante>> GetAllPalestrantesAsync(PageParams pageParams, bool includeEventos = false);
    }
}