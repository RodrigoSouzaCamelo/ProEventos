using AutoMapper;
using ProEventos.Application.DTOs;
using ProEventos.Application.Interfaces;
using ProEventos.Domain.Interfaces.Repositories;
using ProEventos.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProEventos.Application.Services
{
    public class LoteService : ILoteService
    {
        private readonly IMapper _mapper;
        private readonly ILoteRepository _loteRepository;

        public LoteService(IMapper mapper, ILoteRepository lotePersist)
        {
            _mapper = mapper;
            _loteRepository = lotePersist;
        }

        public async Task AddLote(int eventoId, LoteDTO model)
        {
            try
            {
                var lote = _mapper.Map<Lote>(model);
                lote.EventoId = eventoId;

                _loteRepository.Add(lote);

                await _loteRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<LoteDTO>> SaveLotes(int eventoId, IEnumerable<LoteDTO> models)
        {
            try
            {
                var lotes = await _loteRepository.GetLotesByEventoIdAsync(eventoId);
                if (lotes == null) return null;

                foreach (var model in models)
                {
                    if (model.Id == 0) await AddLote(eventoId, model);
                    else await UpdateLote(eventoId, lotes, model);
                }

                var loteRetorno = await _loteRepository.GetLotesByEventoIdAsync(eventoId);

                return _mapper.Map<IEnumerable<LoteDTO>>(loteRetorno);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task UpdateLote(int eventoId, IEnumerable<Lote> lotes, LoteDTO model)
        {
            var lote = lotes.FirstOrDefault(lote => lote.Id == model.Id);
            model.EventoId = eventoId;

            _mapper.Map(model, lote);

            _loteRepository.Update(lote);

            await _loteRepository.SaveChangesAsync();
        }

        public async Task<bool> DeleteLote(LoteDTO loteDTO)
        {
            try
            {
                var lote = _mapper.Map<Lote>(loteDTO);

                _loteRepository.Delete(lote);

                return await _loteRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<LoteDTO>> GetLotesByEventoIdAsync(int eventoId)
        {
            try
            {
                var lotes = await _loteRepository.GetLotesByEventoIdAsync(eventoId);
                if (lotes == null) return null;

                var resultado = _mapper.Map<LoteDTO[]>(lotes);

                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<LoteDTO> GetLoteByIdsAsync(int eventoId, int loteId)
        {
            try
            {
                var lote = await _loteRepository.GetLoteByIdsAsync(eventoId, loteId);
                if (lote == null) return null;

                var resultado = _mapper.Map<LoteDTO>(lote);

                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
