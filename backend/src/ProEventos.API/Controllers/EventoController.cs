using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ProEventos.API.Data;
using ProEventos.API.Models;

namespace ProEventos.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventoController : ControllerBase
    {
        private readonly DataContext _context;

        public EventoController(DataContext context)
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
