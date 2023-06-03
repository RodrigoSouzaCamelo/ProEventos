using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProEventos.Application.Interfaces;
using ProEventos.Domain.Models;
using ProEventos.Persistence.Contexts;

namespace ProEventos.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventoController : ControllerBase
    {
        private readonly IEventoService eventoService;

        public EventoController(IEventoService context)
        {
            this.eventoService = context;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(IEnumerable<Evento>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            try
            {
                var eventos = await eventoService.GetAllEventosAsync();
                
                if(eventos == null) return NotFound();
                
                return Ok(eventos);                
            }
            catch (Exception)
            {
                return StatusCode(500, "Erro ao tentar recuperar eventos!");
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Evento), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var evento = await eventoService.GetEventoByIdAsync(id);
                
                if(evento == null) return NotFound("Evento não encontrado");
                
                return Ok(evento);                
            }
            catch (Exception)
            {
                return StatusCode(500, "Erro ao tentar recuperar evento!");
            }
        }

        [HttpGet("tema/{tema}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(IEnumerable<Evento>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetByTema(string tema)
        {
            try
            {
                var eventos = await eventoService.GetAllEventosByTemaAsync(tema);
                
                if(eventos == null) return NotFound("Evento não encontrado");
                
                return Ok(eventos);                
            }
            catch (Exception)
            {
                return StatusCode(500, "Erro ao tentar recuperar evento!");
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(Evento), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create(Evento evento)
        {
            try
            {
                return Ok(await eventoService.Add(evento));                
            }
            catch (Exception)
            {
                return StatusCode(500, "Erro ao tentar recuperar evento!");
            }
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Evento), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(Evento evento)
        {
            try
            {
                var eventoAtualizado = await eventoService.Update(evento);
                
                if(eventoAtualizado == null) return NotFound("Evento não encontrado");
                
                return base.Ok(eventoAtualizado);                
            }
            catch (Exception)
            {
                return StatusCode(500, "Erro ao tentar recuperar evento!");
            }
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(Evento evento)
        {
            try
            {
                if(await eventoService.Delete(evento)) return NotFound("Evento não encontrado");
                
                return NoContent();                
            }
            catch (Exception)
            {
                return StatusCode(500, "Erro ao tentar recuperar evento!");
            }
        }
    }
}
