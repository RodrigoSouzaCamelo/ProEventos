using Microsoft.EntityFrameworkCore;
using ProEventos.Domain.Enum;
using ProEventos.Domain.Interfaces.Repositories;
using ProEventos.Domain.Models;
using ProEventos.Persistence.Contexts;
using System.Linq;
using System.Threading.Tasks;

namespace ProEventos.Persistence.Repositories
{
    public class PalestranteRepository : Repository<Palestrante>, IPalestranteRepository
    {
        public PalestranteRepository(ProEventosContext context) : base(context)
        {
        }

        public async Task<Palestrante> GetPalestranteByUserIdAsync(int id, bool includeEventos = false)
        {
            IQueryable<Palestrante> query = _context.Set<Palestrante>()
                .Include(p => p.User)
                .Include(p => p.RedesSociais);

            if(includeEventos) {
                query = query.Include(p => p.PalestranteEventos)
                    .ThenInclude(pe => pe.Evento);
            }

            query = query
                .AsNoTracking()
                .OrderBy(p => p.User.PrimeiroNome)
                .Where(p => p.User.Id == id);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<PageList<Palestrante>> GetAllPalestrantesAsync(PageParams pageParams, bool includeEventos = false)
        {
            IQueryable<Palestrante> query = _context.Set<Palestrante>()
                .Include(p => p.User)
                .Include(p => p.RedesSociais);

            if(includeEventos) {
                query = query.Include(p => p.PalestranteEventos)
                    .ThenInclude(pe => pe.Evento);
            }


            query = query
                .OrderBy(p => p.User.PrimeiroNome)
                .Where(p => p.User.PrimeiroNome.ToLower().Contains(pageParams.Term.ToLower()) ||
                            p.User.UltimoNome.ToLower().Contains(pageParams.Term.ToLower()) ||
                            p.MiniCurriculo.ToLower().Contains(pageParams.Term.ToLower()) &&
                            p.User.Funcao == Funcao.Palestrante);

            return await PageList<Palestrante>.CreateAsync(query, pageParams.CurrentPage, pageParams.PageSize);
        }

    }
}