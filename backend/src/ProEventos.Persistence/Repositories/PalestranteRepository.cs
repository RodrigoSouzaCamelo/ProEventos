using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProEventos.Domain.Models;
using ProEventos.Domain.Interfaces.Repositories;
using ProEventos.Persistence.Contexts;

namespace ProEventos.Persistence.Repositories
{
    public class PalestranteRepository : Repository<Palestrante>, IPalestranteRepository
    {
        public PalestranteRepository(ProEventosContext context) : base(context)
        {
        }

        public async Task<Palestrante> GetPalestranteByIdAsync(int id, bool includeEventos = false)
        {
            IQueryable<Palestrante> query = _context.Set<Palestrante>()
                .Include(p => p.RedesSociais);

            if(includeEventos) {
                query = query.Include(p => p.PalestranteEventos)
                    .ThenInclude(pe => pe.Evento);
            }

            query = query.OrderBy(p => p.Nome)
                .Where(p => p.Id == id);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Palestrante>> GetAllPalestrantesAsync(bool includeEventos = false)
        {
            IQueryable<Palestrante> query = _context.Set<Palestrante>()
                .Include(p => p.RedesSociais);

            if(includeEventos) {
                query = query.Include(p => p.PalestranteEventos)
                    .ThenInclude(pe => pe.Evento);
            }

            return await query
                .OrderBy(p => p.Nome)
                .ToListAsync();
        }

        public async Task<IEnumerable<Palestrante>> GetAllPalestrantesByNomeAsync(string nome, bool includeEventos = false)
        {
            IQueryable<Palestrante> query = _context.Set<Palestrante>()
                .Include(p => p.RedesSociais);

            if(includeEventos) {
                query = query.Include(p => p.PalestranteEventos)
                    .ThenInclude(pe => pe.Evento);
            }

            return await query
                .OrderBy(p => p.Nome)
                .Where(p => p.Nome.ToLower().Contains(nome.ToLower()))
                .ToListAsync();
        }

    }
}