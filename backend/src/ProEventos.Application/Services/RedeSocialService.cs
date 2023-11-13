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
    internal class RedeSocialService : IRedeSocialService
    {
        private readonly IMapper _mapper;
        private readonly IRedeSocialRepository _redeSocialRepository;

        public RedeSocialService(IRedeSocialRepository redeSocialRepository, IMapper mapper)
        {
            _mapper = mapper;
            _redeSocialRepository = redeSocialRepository;
        }

        public async Task<RedeSocialDTO[]> SaveByEvento(int eventoId, RedeSocialDTO[] models)
        {
            try
            {
                var redesSociais = await _redeSocialRepository.GetAllRedeSocialByEventoAsync(eventoId);
                if (redesSociais == null) return null;

                foreach (var model in models)
                {
                    if (model.Id == 0)
                    {
                        model.EventoId = eventoId;
                        model.PalestranteId = null;
                        await AddRedeSocial(model);
                    }
                    else
                    {
                        model.EventoId = eventoId;
                        await UpdateLote(redesSociais, model);
                    }
                }

                var loteRetorno = await _redeSocialRepository.GetAllRedeSocialByEventoAsync(eventoId);

                return _mapper.Map<RedeSocialDTO[]>(loteRetorno);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<RedeSocialDTO[]> SaveByPalestrante(int palestranteId, RedeSocialDTO[] models)
        {
            try
            {
                var redesSociais = await _redeSocialRepository.GetAllRedeSocialByPalestranteAsync(palestranteId);
                if (redesSociais == null) return null;

                foreach (var model in models)
                {
                    if (model.Id == 0)
                    {
                        model.EventoId = null;
                        model.PalestranteId = palestranteId;
                        await AddRedeSocial(model);
                    }
                    else
                    {
                        model.PalestranteId = palestranteId;
                        await UpdateLote(redesSociais, model);
                    }
                }

                var loteRetorno = await _redeSocialRepository.GetAllRedeSocialByPalestranteAsync(palestranteId);

                return _mapper.Map<RedeSocialDTO[]>(loteRetorno);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task AddRedeSocial(RedeSocialDTO model)
        {
            try
            {
                var redeSocial = _mapper.Map<RedeSocial>(model);

                _redeSocialRepository.Add(redeSocial);

                await _redeSocialRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task UpdateLote(IEnumerable<RedeSocial> redesSociais, RedeSocialDTO model)
        {
            var redeSocial = redesSociais.FirstOrDefault(redeSocial => redeSocial.Id == model.Id);
            _mapper.Map(model, redeSocial);
            _redeSocialRepository.Update(redeSocial);

            await _redeSocialRepository.SaveChangesAsync();
        }

        public Task<bool> DeleteByEvento(int eventoId, int redeSocialId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteByPalestrante(int palestranteId, int redeSocialId)
        {
            throw new NotImplementedException();
        }

        public Task<RedeSocialDTO[]> GetAllByEventoIdAsync(int eventoId)
        {
            throw new NotImplementedException();
        }

        public Task<RedeSocialDTO[]> GetAllByPalestranteIdAsync(int palestranteId)
        {
            throw new NotImplementedException();
        }

        public Task<RedeSocialDTO> GetRedeSocialEventoByIdsAsync(int eventoId, int RedeSocialId)
        {
            throw new NotImplementedException();
        }

        public Task<RedeSocialDTO> GetRedeSocialPalestranteByIdsAsync(int PalestranteId, int RedeSocialId)
        {
            throw new NotImplementedException();
        }
    }
}
