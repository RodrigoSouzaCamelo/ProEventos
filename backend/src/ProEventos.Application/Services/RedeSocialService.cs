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
                var redesSociais = await _redeSocialRepository.GetAllByEventoIdAsync(eventoId);
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

                var loteRetorno = await _redeSocialRepository.GetAllByEventoIdAsync(eventoId);

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
                var redesSociais = await _redeSocialRepository.GetAllByPalestranteAsync(palestranteId);
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

                var loteRetorno = await _redeSocialRepository.GetAllByPalestranteAsync(palestranteId);

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

        public async Task<bool> DeleteByEvento(int eventoId, int redeSocialId)
        {
            try
            {
                var redeSocial = await _redeSocialRepository.GetByEventoAsync(eventoId, redeSocialId)
                    ?? throw new Exception("Não foi possível encontrar rede social");
                _redeSocialRepository.Delete(redeSocial);

                return await _redeSocialRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteByPalestrante(int palestranteId, int redeSocialId)
        {
            try
            {
                var redeSocial = await _redeSocialRepository.GetByPalestranteAsync(palestranteId, redeSocialId) 
                    ?? throw new Exception("Não foi possível encontrar rede social");
                _redeSocialRepository.Delete(redeSocial);

                return await _redeSocialRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<RedeSocialDTO[]> GetAllByEventoIdAsync(int eventoId)
        {
            try
            {
                var redesSociais = await _redeSocialRepository.GetAllByEventoIdAsync(eventoId);
                if (redesSociais == null) return null;

                return _mapper.Map<RedeSocialDTO[]>(redesSociais);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<RedeSocialDTO[]> GetAllByPalestranteIdAsync(int palestranteId)
        {
            try
            {
                var redesSociais = await _redeSocialRepository.GetAllByPalestranteAsync(palestranteId);
                if (redesSociais == null) return null;

                return _mapper.Map<RedeSocialDTO[]>(redesSociais);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<RedeSocialDTO> GetEventoByIdsAsync(int eventoId, int redeSocialId)
        {
            try
            {
                var redeSocial = await _redeSocialRepository.GetByEventoAsync(eventoId, redeSocialId);
                if (redeSocial == null) return null;

                return _mapper.Map<RedeSocialDTO>(redeSocial);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<RedeSocialDTO> GetPalestranteByIdsAsync(int palestranteId, int redeSocialId)
        {
            try
            {
                var redeSocial = await _redeSocialRepository.GetByPalestranteAsync(palestranteId, redeSocialId);
                if (redeSocial == null) return null;

                return _mapper.Map<RedeSocialDTO>(redeSocial);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
