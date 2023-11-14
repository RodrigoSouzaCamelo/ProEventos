using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProEventos.Domain.Interfaces.Repositories;
using ProEventos.Domain.Models;
using ProEventos.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProEventos.Persistence.Repositories
{
    internal class RedeSocialRepository : Repository<RedeSocial>, IRedeSocialRepository
    {
        protected RedeSocialRepository(ProEventosContext context) : base(context)
        {
        }

        public async Task<List<RedeSocial>> GetAllRedeSocialByEventoAsync(int eventoId)
        {
            return await _context
                .Set<RedeSocial>()
                .AsNoTracking()
                .Where(rs => rs.EventoId == eventoId)
                .ToListAsync();
        }

        public async Task<List<RedeSocial>> GetAllRedeSocialByPalestranteAsync(int palestranteId)
        {
            return await _context
                .Set<RedeSocial>()
                .AsNoTracking()
                .Where(rs => rs.PalestranteId == palestranteId)
                .ToListAsync();
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
