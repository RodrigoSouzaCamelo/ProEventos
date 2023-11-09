using ProEventos.Domain.Models;
using System.Threading.Tasks;

namespace ProEventos.Domain.Interfaces.Repositories
{
    public interface IRedeSocialRepository : IRepository<RedeSocial>
    {
        Task<RedeSocial[]> GetAllRedeSocialByEventoAsync(int eventoId);
        Task<RedeSocial> GetRedeSocialByEventoAsync(int eventoId, int id);
        Task<RedeSocial[]> GetAllRedeSocialByPalestranteAsync(int palestranteId);
        Task<RedeSocial> GetRedeSocialByPalestranteAsync(int palestranteId, int id);
    }
}
