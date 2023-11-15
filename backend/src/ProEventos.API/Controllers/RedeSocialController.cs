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
                if(!await EhAutorDoEvento(eventoId)) return Unauthorized();

                var redeSociais = await _redeSocialService.GetAllByEventoIdAsync(eventoId);
                
                if (redeSociais == null) return NoContent();

                return Ok(redeSociais);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
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

                if(palestrante == null) return Unauthorized();

                var redeSociais = await _redeSocialService.GetAllByPalestranteIdAsync(palestranteId);

                if (redeSociais == null) return NoContent();

                return Ok(redeSociais);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar recuperar rede sociais. Erro: {ex.Message}");
            }
        }

        [HttpPut("{eventoId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(RedeSocialDTO[]), StatusCodes.Status200OK)]
        public async Task<IActionResult> Add(int eventoId, RedeSocialDTO[] models)
        {
            try
            {
                var redeSociais = await _redeSocialService.SaveByEvento(eventoId, models);
                if (redeSociais == null) return NoContent();

                return Ok(redeSociais);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar salvar lotes. Erro: {ex.Message}");
            }
        }

        [HttpDelete("{loteId}/evento/{eventoId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(int eventoId, int loteId)
        {
            try
            {
                var lote = await _loteService.GetLoteByIdsAsync(eventoId, loteId);
                if (lote == null) return NoContent();

                return await _loteService.DeleteLote(lote)
                       ? Ok(new { message = "Lote Deletado" })
                       : throw new Exception("Ocorreu um problem não específico ao tentar deletar Lote.");
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar deletar lotes. Erro: {ex.Message}");
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
