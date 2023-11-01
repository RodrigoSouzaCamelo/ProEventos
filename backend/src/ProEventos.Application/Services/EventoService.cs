using AutoMapper;
using ProEventos.Application.DTOs;
using ProEventos.Application.Interfaces;
using ProEventos.Domain.Interfaces.Repositories;
using ProEventos.Domain.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ProEventos.Application.Services
{
    public class EventoService : IEventoService
    {
        private readonly IMapper _mapper;
        private readonly IEventoRepository _eventoRepository;

        public EventoService(IMapper mapper, IEventoRepository eventoRepository)
        {
            _mapper = mapper;
            _eventoRepository = eventoRepository;
        }

        public async Task<PageList<EventoDTO>> GetAllEventosAsync(int userId, PageParams pageParams, bool includeEventos = false)
        {
            try
            {
                var eventosPaginados = await _eventoRepository
                    .GetAllEventosAsync(userId, pageParams, includeEventos);

                var resultado = _mapper.Map<PageList<EventoDTO>>(eventosPaginados);

                resultado.PageSize = eventosPaginados.PageSize;
                resultado.CurrentPage = eventosPaginados.CurrentPage;
                resultado.TotalPages = eventosPaginados.TotalPages;
                resultado.PageSize = eventosPaginados.PageSize;
                resultado.TotalCount = eventosPaginados.TotalCount;

                return resultado;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<EventoDTO> GetEventoByIdAsync(int userId, int id, bool includeEventos = false)
        {
            try
            {
                var result = await _eventoRepository
                    .GetEventoByIdAsync(userId, id, includeEventos);

                return _mapper.Map<EventoDTO>(result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<EventoDTO> Add(int userId, EventoDTO eventoDTO)
        {
            try
            {
                var evento = _mapper.Map<Evento>(eventoDTO);
                evento.UserId = userId;
                _eventoRepository.Add(evento);

                if(await _eventoRepository.SaveChangesAsync())
                    return _mapper.Map<EventoDTO>(evento);

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<EventoDTO> Update(int userId, EventoDTO eventoDTO)
        {
            try
            {
                var evento = _mapper.Map<Evento>(eventoDTO);
                evento.UserId = userId;
                _eventoRepository.Update(evento);

                if(await _eventoRepository.SaveChangesAsync())
                    return _mapper.Map<EventoDTO>(evento);

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> Delete(int userId, EventoDTO eventoDTO)
        {
            try
            {
                var evento = _mapper.Map<Evento>(eventoDTO);
                evento.UserId = userId;
                _eventoRepository.Delete(evento);
                return await _eventoRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteRange(int userId, EventoDTO[] eventosDTO)
        {
            try
            {
                var eventos = _mapper.Map<Evento[]>(eventosDTO);
                eventos = eventos.Select(e => {
                    e.UserId = userId;
                    return e;
                }).ToArray();

                _eventoRepository.DeleteRange(eventos);
                return !await _eventoRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}