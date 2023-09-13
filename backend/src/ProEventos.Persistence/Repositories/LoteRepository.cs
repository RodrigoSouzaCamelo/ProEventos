using Microsoft.EntityFrameworkCore;
using ProEventos.Domain.Interfaces.Repositories;
using ProEventos.Domain.Models;
using ProEventos.Persistence.Contexts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProEventos.Persistence.Repositories
{
    public class LoteRepository : Repository<Lote>, ILoteRepository
    {
        public LoteRepository(ProEventosContext context) : base(context)
        {
        }

        public async Task<Lote> GetLoteByIdsAsync(int eventoId, int id)
        {
            IQueryable<Lote>  query = _context.Set<Lote>().AsNoTracking()
                         .Where(lote => lote.EventoId == eventoId && lote.Id == id);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Lote>> GetLotesByEventoIdAsync(int eventoId)
        {
            return await _context.Set<Lote>().AsNoTracking()
                .Where(lote => lote.EventoId == eventoId)
                .ToListAsync();
        }
    }
}
