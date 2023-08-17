using AutoMapper;
using ProEventos.Application.DTOs;
using ProEventos.Application.Interfaces;
using ProEventos.Domain.Interfaces.Repositories;
using ProEventos.Domain.Models;
using System;
using System.Collections.Generic;
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

        public async Task<IEnumerable<EventoDTO>> GetAllEventosAsync(bool includeEventos = false)
        {
            try
            {
                var result = await _eventoRepository
                    .GetAllEventosAsync(includeEventos);

                return _mapper.Map<IEnumerable<EventoDTO>>(result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<EventoDTO>> GetAllEventosByTemaAsync(string tema, bool includeEventos = false)
        {
            try
            {
                var result = await _eventoRepository
                    .GetAllEventosByTemaAsync(tema, includeEventos);

                return _mapper.Map<IEnumerable<EventoDTO>>(result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<EventoDTO> GetEventoByIdAsync(int id, bool includeEventos = false)
        {
            try
            {
                var result = await _eventoRepository
                    .GetEventoByIdAsync(id, includeEventos);

                return _mapper.Map<EventoDTO>(result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<EventoDTO> Add(EventoDTO eventoDTO)
        {
            try
            {
                var evento = _mapper.Map<Evento>(eventoDTO);
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

        public async Task<EventoDTO> Update(EventoDTO eventoDTO)
        {
            try
            {
                var evento = _mapper.Map<Evento>(eventoDTO);
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

        public async Task<bool> Delete(EventoDTO eventoDTO)
        {
            try
            {
                var evento = _mapper.Map<Evento>(eventoDTO);
                _eventoRepository.Delete(evento);
                return !await _eventoRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteRange(EventoDTO[] eventosDTO)
        {
            try
            {
                var eventos = _mapper.Map<Evento[]>(eventosDTO);
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