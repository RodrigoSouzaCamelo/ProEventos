using Microsoft.EntityFrameworkCore;
using ProEventos.Domain.Interfaces.Repositories;
using ProEventos.Domain.Models;
using ProEventos.Persistence.Contexts;
using System.Linq;
using System.Threading.Tasks;

namespace ProEventos.Persistence.Repositories
{
    public class RedeSocialRepository : Repository<RedeSocial>, IRedeSocialRepository
    {
        public RedeSocialRepository(ProEventosContext context) : base(context)
        {
        }

        public async Task<RedeSocial[]> GetAllByEventoIdAsync(int eventoId)
        {
            return await _context
                .Set<RedeSocial>()
                .AsNoTracking()
                .Where(rs => rs.EventoId == eventoId)
                .ToArrayAsync();
        }

        public async Task<RedeSocial[]> GetAllByPalestranteIdAsync(int palestranteId)
        {
            return await _context
                .Set<RedeSocial>()
                .AsNoTracking()
                .Where(rs => rs.PalestranteId == palestranteId)
                .ToArrayAsync();
        }

        public async Task<RedeSocial> GetByEventoAsync(int eventoId, int id)
        {
            return await _context
                .Set<RedeSocial>()
                .AsNoTracking()
                .FirstOrDefaultAsync(rs => rs.EventoId == eventoId && rs.Id == id);
        }

        public async Task<RedeSocial> GetByPalestranteAsync(int palestranteId, int id)
        {
            return await _context
                .Set<RedeSocial>()
                .AsNoTracking()
                .FirstOrDefaultAsync(rs => rs.PalestranteId == palestranteId && rs.Id == id);
        }
    }
}
