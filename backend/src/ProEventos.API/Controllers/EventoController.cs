﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProEventos.Application.DTOs;
using ProEventos.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ProEventos.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventoController : ControllerBase
    {
        private readonly IEventoService _eventoService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public EventoController(IEventoService context, IWebHostEnvironment webHostEnvironment)
        {
            _eventoService = context;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(IEnumerable<EventoDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            try
            {
                var eventos = await _eventoService.GetAllEventosAsync();
                
                if(eventos == null) return NoContent();
                
                return Ok(eventos);                
            }
            catch (Exception)
            {
                return StatusCode(500, "Erro ao tentar recuperar eventos!");
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(EventoDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var evento = await _eventoService.GetEventoByIdAsync(id);
                
                if(evento == null) return NoContent();
                
                return Ok(evento);                
            }
            catch (Exception)
            {
                return StatusCode(500, "Erro ao tentar recuperar evento!");
            }
        }

        [HttpGet("tema/{tema}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(IEnumerable<EventoDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetByTema(string tema)
        {
            try
            {
                var eventos = await _eventoService.GetAllEventosByTemaAsync(tema);
                
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
                return Ok(await _eventoService.Add(evento));                
            }
            catch (Exception)
            {
                return StatusCode(500, "Erro ao tentar recuperar evento!");
            }
        }

        [HttpPost("upload/image/{eventoId}")]
        [ProducesResponseType(typeof(EventoDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UploadImage(int eventoId)
        {
            try
            {
                var evento = await _eventoService.GetEventoByIdAsync(eventoId, true);
                
                if(evento == null) return NoContent();

                var file = Request.Form.Files[0];
                if(file.Length > 0)
                {
                    DeleteImage(evento.ImagemURL);
                    //evento.ImageURL = SaveImage(file);
                }

                var retorno = await _eventoService.Update(evento);

                return Ok();
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
                var eventoAtualizado = await _eventoService.Update(evento);
                
                if(eventoAtualizado == null) return NotFound("Evento não encontrado");
                
                return base.Ok(eventoAtualizado);                
            }
            catch (Exception)
            {
                return StatusCode(500, "Erro ao tentar recuperar evento!");
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var evento = await _eventoService.GetEventoByIdAsync(id, true);
                
                if (evento == null) return NotFound();
                
                if(!await _eventoService.Delete(evento)) return StatusCode(500);
                
                return NoContent();                
            }
            catch (Exception)
            {
                return StatusCode(500, "Erro ao tentar recuperar evento!");
            }
        }

        [NonAction]
        private void DeleteImage(string imageName)
        {
            var imagePath = Path.Combine(_webHostEnvironment.ContentRootPath, @"Resources/Images", imageName);
            if(System.IO.File.Exists(imagePath))
                System.IO.File.Delete(imagePath);

        }
    }
}
