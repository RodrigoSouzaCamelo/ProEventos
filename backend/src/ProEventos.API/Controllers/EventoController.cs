using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ProEventos.Domain;
using ProEventos.Persistence;

namespace ProEventos.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventoController : ControllerBase
    {
        private readonly ProEventosContext _context;

        public EventoController(ProEventosContext context)
        {
            this._context = context;
        }

        [HttpGet]
        public IQueryable<Evento> Get()
        {
            return _context.Set<Evento>().AsQueryable();
        }

        [HttpGet("{id}")]
        public Evento Get(int id)
        {
            return _context.Set<Evento>().FirstOrDefault(e => e.EventoId == id);
        }
    }
}
