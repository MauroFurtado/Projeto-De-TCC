using AutoMapper;
using ResrvaDeSala_API.DTOs;
using ResrvaDeSala_API.Models;

namespace ReservaDeSala_API.Data
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<UsuarioModel, UsuarioDto>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.Nome, o => o.MapFrom(s => s.Nome))
                .ForMember(d => d.Email, o => o.MapFrom(s => s.Email))
                .ForMember(d => d.CriadoEm, o => o.MapFrom(s => s.CriadoEm));

            CreateMap<UsuarioCreateDto, UsuarioModel>()
                .ForMember(d => d.Nome, o => o.MapFrom(s => s.Nome))
                .ForMember(d => d.Email, o => o.MapFrom(s => s.Email))
                .ForMember(d => d.Senha, o => o.MapFrom(s => s.Senha));

            CreateMap<UsuarioDto, UsuarioModel>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.Nome, o => o.MapFrom(s => s.Nome))
                .ForMember(d => d.Email, o => o.MapFrom(s => s.Email))
                .ForMember(d => d.CriadoEm, o => o.MapFrom(s => s.CriadoEm));

            CreateMap<SalaModel, SalaDto>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.Nome, o => o.MapFrom(s => s.Nome))
                .ForMember(d => d.Capacidade, o => o.MapFrom(s => s.Capacidade))
                .ForMember(d => d.Disponivel, o => o.MapFrom(s => s.Disponivel));

            CreateMap<ReservaModel, ReservaDto>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.SalaId, o => o.MapFrom(s => s.SalaId))
                .ForMember(d => d.UsuarioId, o => o.MapFrom(s => s.UsuarioId))
                .ForMember(d => d.DataReserva, o => o.MapFrom(s => s.DataReserva))
                .ForMember(d => d.HoraInicio, o => o.MapFrom(s => s.HoraInicio))
                .ForMember(d => d.HoraFim, o => o.MapFrom(s => s.HoraFim))
                .ForMember(d => d.CriadoEm, o => o.MapFrom(s => s.CriadoEm));

            CreateMap<ReservaCreateDto, ReservaModel>()
                .ForMember(d => d.SalaId, o => o.MapFrom(s => s.SalaId))
                .ForMember(d => d.UsuarioId, o => o.MapFrom(s => s.UsuarioId))
                .ForMember(d => d.DataReserva, o => o.MapFrom(s => s.DataReserva))
                .ForMember(d => d.HoraInicio, o => o.MapFrom(s => s.HoraInicio))
                .ForMember(d => d.HoraFim, o => o.MapFrom(s => s.HoraFim));
        }
    }
}