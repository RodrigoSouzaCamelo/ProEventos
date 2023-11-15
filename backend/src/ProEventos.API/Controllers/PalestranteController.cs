using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProEventos.API.Extensions;
using ProEventos.Application.DTOs;
using ProEventos.Application.Interfaces;
using ProEventos.Domain.Models;
using System;
using System.Threading.Tasks;

namespace ProEventos.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PalestranteController : ControllerBase
    {
        private readonly IPalestranteService _palestranteService;

        public PalestranteController(IPalestranteService palestranteService)
        {
            _palestranteService = palestranteService;
        }

        [HttpGet("todos")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(PageList<EventoDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromQuery] PageParams pageParams)
        {
            try
            {
                var palestrantes = await _palestranteService.GetAllPalestrantesAsync(pageParams);

                if (palestrantes == null) return NoContent();

                Response.AddPagination(palestrantes);

                return Ok(palestrantes);
            }
            catch (Exception)
            {
                return StatusCode(500, "Erro ao tentar recuperar palestrantes!");
            }
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(EventoDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById()
        {
            try
            {
                var palestrante = await _palestranteService.GetPalestranteByUserIdAsync(User.GetUserId());

                if (palestrante == null) return NoContent();

                return Ok(palestrante);
            }
            catch (Exception)
            {
                return StatusCode(500, "Erro ao tentar recuperar palestrante!");
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(PalestranteAddDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create(PalestranteAddDTO palestranteDTO)
        {
            try
            {
                var palestrante = await _palestranteService.GetPalestranteByUserIdAsync(User.GetUserId());

                if (palestrante != null) return Ok(palestrante);

                return Ok(await _palestranteService.Add(User.GetUserId(), palestranteDTO));
            }
            catch (Exception)
            {
                return StatusCode(500, "Erro ao tentar recuperar palestrante!");
            }
        }


        [HttpPut]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(PalestranteUpdateDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(PalestranteUpdateDTO palestranteDTO)
        {
            try
            {
                var palestranteAtualizado = await _palestranteService.Update(User.GetUserId(), palestranteDTO);

                if (palestranteAtualizado == null) return NotFound("Palestrante não encontrado");

                return base.Ok(palestranteAtualizado);
            }
            catch (Exception)
            {
                return StatusCode(500, "Erro ao tentar recuperar palestrante!");
            }
        }
    }
}
