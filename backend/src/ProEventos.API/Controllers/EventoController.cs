using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProEventos.Application.DTOs;
using ProEventos.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        [ProducesResponseType(typeof(IEnumerable<EventoDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            try
            {
                var eventos = await eventoService.GetAllEventosAsync();
                
                if(eventos == null) return NoContent();

                var eventosRetorno = eventos.Select(evento => new EventoDTO() {
                    Id = evento.Id,
                    Local = evento.Local,
                    Data = evento.Data.ToString(),
                    Tema = evento.Tema,
                    QtdPessoas = evento.QtdPessoas,
                    ImagemURL = evento.ImagemURL,
                    Telefone = evento.Telefone,
                    Email = evento.Email,
                });
                
                return Ok(eventosRetorno);                
            }
            catch (Exception)
            {
                return StatusCode(500, "Erro ao tentar recuperar eventos!");
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(EventoDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var evento = await eventoService.GetEventoByIdAsync(id);
                
                if(evento == null) return NoContent();
                
                return Ok(evento);                
            }
            catch (Exception)
            {
                return StatusCode(500, "Erro ao tentar recuperar evento!");
            }
        }

        [HttpGet("tema/{tema}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(IEnumerable<EventoDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetByTema(string tema)
        {
            try
            {
                var eventos = await eventoService.GetAllEventosByTemaAsync(tema);
                
                if(eventos == null) return NoContent();
                
                return Ok(eventos);                
            }
            catch (Exception)
            {
                return StatusCode(500, "Erro ao tentar recuperar evento!");
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(EventoDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create(EventoDTO evento)
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
        [ProducesResponseType(typeof(EventoDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(EventoDTO evento)
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
        public async Task<IActionResult> Delete(EventoDTO evento)
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
