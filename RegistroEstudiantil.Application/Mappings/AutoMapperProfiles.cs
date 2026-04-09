using AutoMapper;
using RegistroEstudiantil.Application.DTOs;
using RegistroEstudiantil.Domain.Entities;



namespace RegistroEstudiantil.Application.Utilidades
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            ConfigurarMapeosEstudiante();

        }
        private void ConfigurarMapeosEstudiante()
        {
            CreateMap<EstudianteCreacionDTO, Estudiante>();
            CreateMap<Estudiante, EstudianteDTO>();
            CreateMap<EstudianteUpdateDTO, Estudiante>()
    .ForMember(d => d.Id, opt => opt.Ignore())
    .ForMember(d => d.UsuarioId, opt => opt.Ignore());

        }


    }
}


