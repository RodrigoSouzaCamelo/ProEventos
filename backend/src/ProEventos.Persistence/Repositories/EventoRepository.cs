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

        public async Task<PageList<Evento>> GetAllEventosAsync(int userId, PageParams pageParams, bool includePalestrantes = false)
        {
            IQueryable<Evento> query = _context.Set<Evento>()
                .Include(e => e.Lotes)
                .Include(e => e.RedesSociais);

            if(includePalestrantes) {
                query = query
                    .Include(e => e.PalestranteEventos)
                    .ThenInclude(pe => pe.Palestrante);
            }

            query = query
                .Where(e => (e.Tema.ToLower().Contains(pageParams.Term.ToLower()) ||
                             e.Local.ToLower().Contains(pageParams.Term.ToLower())) && e.UserId == userId)
                .OrderBy(e => e.Tema);

            return await PageList<Evento>.CreateAsync(query, pageParams.CurrentPage, pageParams.PageSize);
        }
    }
}