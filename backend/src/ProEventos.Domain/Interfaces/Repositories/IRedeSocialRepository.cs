using ProEventos.Domain.Models;
using System.Threading.Tasks;

namespace ProEventos.Domain.Interfaces.Repositories
{
    public interface IRedeSocialRepository : IRepository<RedeSocial>
    {
        Task<RedeSocial[]> GetAllByEventoIdAsync(int eventoId);
        Task<RedeSocial> GetByEventoAsync(int eventoId, int id);
        Task<RedeSocial[]> GetAllByPalestranteIdAsync(int palestranteId);
        Task<RedeSocial> GetByPalestranteAsync(int palestranteId, int id);
    }
}
