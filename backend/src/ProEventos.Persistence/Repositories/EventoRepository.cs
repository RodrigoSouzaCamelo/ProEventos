using Microsoft.EntityFrameworkCore;
using ProEventos.Domain.Interfaces.Repositories;
using ProEventos.Domain.Models;
using ProEventos.Persistence.Contexts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProEventos.Persistence.Repositories
{
    public class EventoRepository : Repository<Evento>, IEventoRepository
    {
        public EventoRepository(ProEventosContext context) : base(context)
        {
        }

        public async Task<Evento> GetEventoByIdAsync(int userId, int id, bool includePalestrantes = false)
        {
            IQueryable<Evento> query = _context.Set<Evento>()
                .Include(e => e.Lotes)
                .Include(e => e.RedesSociais);

            if(includePalestrantes) {
                query = query
                    .Include(e => e.PalestranteEventos)
                    .ThenInclude(pe => pe.Palestrante);
            }

            return await query
                .AsNoTracking()
                .OrderBy(e => e.Tema)
                .Where(e => e.Id == id && e.UserId == userId)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Evento>> GetAllEventosAsync(int userId, bool includePalestrantes = false)
        {
            IQueryable<Evento> query = _context.Set<Evento>()
                .Include(e => e.Lotes)
                .Include(e => e.RedesSociais);

            if(includePalestrantes) {
                query = query
                    .Include(e => e.PalestranteEventos)
                    .ThenInclude(pe => pe.Palestrante);
            }

            return await query
                .Where(e => e.UserId == userId)
                .OrderBy(e => e.Tema)
                .ToListAsync();
        }

        public async Task<IEnumerable<Evento>> GetAllEventosByTemaAsync(int userId, string tema, bool includePalestrantes = false)
        {
            IQueryable<Evento> query = _context.Set<Evento>()
                .Include(e => e.Lotes)
                .Include(e => e.RedesSociais);

            if(includePalestrantes) {
                query = query
                    .Include(e => e.PalestranteEventos)
                    .ThenInclude(pe => pe.Palestrante);
            }

            return await query
                .OrderBy(e => e.Tema)
                .Where(e => e.Tema.ToLower().Contains(tema.ToLower()) && e.UserId == userId)
                .ToListAsync();
        }
    }
}