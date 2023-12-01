using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProEventos.Application.DTOs;
using ProEventos.Application.Interfaces;
using ProEventos.Application.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using ProEventos.API.Extensions;

namespace ProEventos.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RedeSocialController : ControllerBase
    {
        private readonly IEventoService _eventoService;
        private readonly IRedeSocialService _redeSocialService;
        private readonly IPalestranteService _palestranteService;

        public RedeSocialController(IRedeSocialService redeSocialService, IEventoService eventoService, IPalestranteService palestranteService)
        {
            _redeSocialService = redeSocialService;
            _eventoService = eventoService;
            _palestranteService = palestranteService;
        }

        [HttpGet("evento/{eventoId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(RedeSocialDTO[]), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByEventoId(int eventoId)
        {
            try
            {
                if (!await EhAutorDoEvento(eventoId)) return Unauthorized();

                var redeSociais = await _redeSocialService.GetAllByEventoIdAsync(eventoId);

                if (redeSociais == null) return NoContent();

                return Ok(redeSociais);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar recuperar rede sociais. Erro: {ex.Message}");
            }
        }

        [HttpGet("palestrante")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(RedeSocialDTO[]), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByPalestrante()
        {
            try
            {
                var palestranteId = User.GetUserId();
                var palestrante = await _palestranteService.GetPalestranteByUserIdAsync(palestranteId);

                if (palestrante == null) return Unauthorized();

                var redeSociais = await _redeSocialService.GetAllByPalestranteIdAsync(palestranteId);

                if (redeSociais == null) return NoContent();

                return Ok(redeSociais);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar recuperar rede sociais. Erro: {ex.Message}");
            }
        }

        [HttpPut("evento/{eventoId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(RedeSocialDTO[]), StatusCodes.Status200OK)]
        public async Task<IActionResult> SaveByEvento(int eventoId, RedeSocialDTO[] models)
        {
            try
            {
                if (!await EhAutorDoEvento(eventoId)) return Unauthorized();

                var redeSociais = await _redeSocialService.SaveByEvento(eventoId, models);
                if (redeSociais == null) return NoContent();

                return Ok(redeSociais);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar salvar rede sociais. Erro: {ex.Message}");
            }
        }

        [HttpPut("palestrante")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(RedeSocialDTO[]), StatusCodes.Status200OK)]
        public async Task<IActionResult> SaveByPalestrante(RedeSocialDTO[] dtos)
        {
            try
            {
                var palestranteId = User.GetUserId();
                var palestrante = await _palestranteService.GetPalestranteByUserIdAsync(palestranteId);

                if (palestrante == null) return Unauthorized();

                var redeSociais = await _redeSocialService.SaveByPalestrante(palestranteId, dtos);

                if (redeSociais == null) return NoContent();

                return Ok(redeSociais);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar salvar rede sociais. Erro: {ex.Message}");
            }
        }

        [HttpDelete("evento/{eventoId}/{redeSocialId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteByEvento(int eventoId, int redeSocialId)
        {
            try
            {
                if (!await EhAutorDoEvento(eventoId)) return Unauthorized();

                var redeSocial = await _redeSocialService.GetByEventoIdAsync(eventoId, redeSocialId);
                if (redeSocial == null) return NoContent();

                return await _redeSocialService.DeleteByEvento(eventoId, redeSocialId)
                       ? Ok(new { message = "Rede social deletada" })
                       : throw new Exception("Ocorreu um problema não específico ao tentar deletar rede social.");
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar deletar rede social. Erro: {ex.Message}");
            }
        }

        [HttpDelete("palestrante/{redeSocialId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteByPalestrante(int redeSocialId)
        {
            try
            {
                var palestranteId = User.GetUserId();
                var palestrante = await _palestranteService.GetPalestranteByUserIdAsync(palestranteId);

                if (palestrante == null) return Unauthorized();

                var redeSocial = await _redeSocialService.GetByPalestranteIdAsync(palestranteId, redeSocialId);
                if (redeSocial == null) return NoContent();

                return await _redeSocialService.DeleteByPalestrante(palestranteId, redeSocialId)
                       ? Ok(new { message = "Rede social deletada" })
                       : throw new Exception("Ocorreu um problema não específico ao tentar deletar rede social.");
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar deletar rede social. Erro: {ex.Message}");
            }
        }

        [NonAction]
        public async Task<bool> EhAutorDoEvento(int eventoId)
        {
            var evento = await _eventoService.GetEventoByIdAsync(User.GetUserId(), eventoId, false);
            return evento != null;

        }
    }
}
