using AutoMapper;
using ProEventos.Application.DTOs;
using ProEventos.Application.Interfaces;
using ProEventos.Domain.Interfaces.Repositories;
using ProEventos.Domain.Models;
using System;
using System.Threading.Tasks;

namespace ProEventos.Application.Services
{
    public class PalestranteService : IPalestranteService
    {
        private readonly IMapper _mapper;
        private readonly IPalestranteRepository _palestranteRepository;

        public PalestranteService(IMapper mapper, IPalestranteRepository palestranteRepository)
        {
            _mapper = mapper;
            _palestranteRepository = palestranteRepository;
        }

        public async Task<PalestranteDTO> Add(int userId, PalestranteAddDTO palestranteDTO)
        {
            try
            {
                var palestrante = _mapper.Map<Palestrante>(palestranteDTO);
                palestrante.UserId = userId;
                _palestranteRepository.Add(palestrante);

                if (!await _palestranteRepository.SaveChangesAsync()) return null;

                var palestranteRetorno = await _palestranteRepository.GetPalestranteByUserIdAsync(userId);
                return _mapper.Map<PalestranteDTO>(palestranteRetorno);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<PalestranteDTO> Update(int userId, PalestranteUpdateDTO palestranteDTO)
        {
            try
            {
                var palestrante = _mapper.Map<Palestrante>(palestranteDTO);
                palestrante.UserId = userId;
                _palestranteRepository.Update(palestrante);

                if (!await _palestranteRepository.SaveChangesAsync()) return null;

                var palestranteRetorno = await _palestranteRepository.GetPalestranteByUserIdAsync(userId);             
                return _mapper.Map<PalestranteDTO>(palestranteRetorno);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<PageList<PalestranteDTO>> GetAllPalestrantesAsync(PageParams pageParams, bool includeEventos = false)
        {
            try
            {
                var palestrantesPaginados = await _palestranteRepository
                    .GetAllPalestrantesAsync(pageParams, includeEventos);

                var resultado = _mapper.Map<PageList<PalestranteDTO>>(palestrantesPaginados);

                resultado.PageSize = palestrantesPaginados.PageSize;
                resultado.CurrentPage = palestrantesPaginados.CurrentPage;
                resultado.TotalPages = palestrantesPaginados.TotalPages;
                resultado.PageSize = palestrantesPaginados.PageSize;
                resultado.TotalCount = palestrantesPaginados.TotalCount;

                return resultado;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<PalestranteDTO> GetPalestranteByUserIdAsync(int userId, bool includeEventos = false)
        {
            try
            {
                var result = await _palestranteRepository
                    .GetPalestranteByUserIdAsync(userId,includeEventos);

                return _mapper.Map<PalestranteDTO>(result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
