using AutoMapper;
using ProEventos.Application.DTOs;
using ProEventos.Domain.Identity;
using ProEventos.Domain.Models;

namespace ProEventos.Application.Helpers
{
    public class ProEventosProfile : Profile
    {
        public ProEventosProfile()
        {
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<Lote, LoteDTO>().ReverseMap();
            CreateMap<Evento, EventoDTO>().ReverseMap();
            CreateMap<User, UserLoginDTO>().ReverseMap();
            CreateMap<User, UserUpdateDTO>().ReverseMap();
            CreateMap<RedeSocial, RedeSocialDTO>().ReverseMap();
            CreateMap<Palestrante, PalestranteDTO>().ReverseMap();
            CreateMap<Palestrante, PalestranteAddDTO>().ReverseMap();
            CreateMap<Palestrante, PalestranteUpdateDTO>().ReverseMap();
        }
    }
}