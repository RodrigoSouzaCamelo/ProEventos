using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProEventos.Domain.Models;

namespace ProEventos.Domain.Interfaces.Repositories
{
    public interface IPalestranteRepository : IRepository<Palestrante>
    {
        Task<Palestrante> GetPalestranteByIdAsync(int id, bool includeEventos = false);
        Task<IEnumerable<Palestrante>> GetAllPalestrantesAsync(bool includeEventos = false);
        Task<IEnumerable<Palestrante>> GetAllPalestrantesByNomeAsync(string nome, bool includeEventos = false);
    }
}