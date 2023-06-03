using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProEventos.Application.Interfaces;
using ProEventos.Domain.Interfaces.Repositories;
using ProEventos.Domain.Models;

namespace ProEventos.Application.Services
{
    public class EventoService : IEventoService
    {
        private readonly IEventoRepository _eventoRepository;

        public EventoService(IEventoRepository eventoRepository)
        {
            this._eventoRepository = eventoRepository;
        }

        public async Task<IEnumerable<Evento>> GetAllEventosAsync(bool includeEventos = false)
        {
            try
            {
                return await _eventoRepository
                    .GetAllEventosAsync(includeEventos);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<Evento>> GetAllEventosByTemaAsync(string tema, bool includeEventos = false)
        {
            try
            {
                return await _eventoRepository
                    .GetAllEventosByTemaAsync(tema, includeEventos);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Evento> GetEventoByIdAsync(int id, bool includeEventos = false)
        {
            try
            {
                return await _eventoRepository
                    .GetEventoByIdAsync(id, includeEventos);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Evento> Add(Evento evento)
        {
            try
            {
                _eventoRepository.Add(evento);

                if(await _eventoRepository.SaveChangesAsync())
                    return evento;

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Evento> Update(Evento evento)
        {
            try
            {
                _eventoRepository.Update(evento);

                if(await _eventoRepository.SaveChangesAsync())
                    return evento;

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> Delete(Evento evento)
        {
            try
            {
                _eventoRepository.Delete(evento);
                return !await _eventoRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteRange(Evento[] eventos)
        {
            try
            {
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