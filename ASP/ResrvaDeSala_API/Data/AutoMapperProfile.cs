using System;
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
                .ForMember(d => d.Id, o => o.Ignore())
                .ForMember(d => d.CriadoEm, o => o.Ignore());

            CreateMap<SalaModel, SalaDto>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.Nome, o => o.MapFrom(s => s.Nome))
                .ForMember(d => d.Tipo, o => o.MapFrom(s => FormatTipoSala(s.Tipo)))
                .ForMember(d => d.Localizacao, o => o.MapFrom(s => s.Localizacao))
                .ForMember(d => d.Capacidade, o => o.MapFrom(s => s.Capacidade))
                .ForMember(d => d.Disponivel, o => o.MapFrom(s => s.Disponivel));

            CreateMap<SalaDto, SalaModel>()
                .ForMember(d => d.Id, o => o.Ignore())
                .ForMember(d => d.Tipo, o => o.MapFrom(s => ParseTipoSala(s.Tipo)))
                .ForMember(d => d.Localizacao, o => o.MapFrom(s => s.Localizacao))
                .ForMember(d => d.Capacidade, o => o.MapFrom(s => s.Capacidade))
                .ForMember(d => d.Disponivel, o => o.MapFrom(s => s.Disponivel ?? true))
                .ForMember(d => d.CriadoEm, o => o.Ignore());

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

            CreateMap<ReservaDto, ReservaModel>()
                .ForMember(d => d.Id, o => o.Ignore())
                .ForMember(d => d.SalaId, o => o.MapFrom(s => s.SalaId))
                .ForMember(d => d.UsuarioId, o => o.MapFrom(s => s.UsuarioId))
                .ForMember(d => d.DataReserva, o => o.MapFrom(s => s.DataReserva))
                .ForMember(d => d.HoraInicio, o => o.MapFrom(s => s.HoraInicio))
                .ForMember(d => d.HoraFim, o => o.MapFrom(s => s.HoraFim))
                .ForMember(d => d.CriadoEm, o => o.Ignore())
                .ForMember(d => d.Sala, o => o.Ignore())
                .ForMember(d => d.Usuario, o => o.Ignore());
        }

        private static string FormatTipoSala(TipoSala tipo) => tipo switch
        {
            TipoSala.Laboratorio => "laboratorio",
            TipoSala.Aula => "aula",
            TipoSala.Reuniao => "reuniao",
            TipoSala.Auditorio => "auditório",
            _ => throw new ArgumentOutOfRangeException(nameof(tipo), tipo, "Tipo de sala inválido"),
        };

        private static TipoSala ParseTipoSala(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("O tipo da sala é obrigatório", nameof(value));
            }

            return value.Trim().ToLowerInvariant() switch
            {
                "laboratorio" => TipoSala.Laboratorio,
                "aula" => TipoSala.Aula,
                "reuniao" => TipoSala.Reuniao,
                "auditorio" => TipoSala.Auditorio,
                "auditório" => TipoSala.Auditorio,
                _ => throw new ArgumentException($"Tipo da sala '{value}' é inválido.", nameof(value)),
            };
        }
    }
}