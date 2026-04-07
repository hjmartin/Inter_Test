using AutoMapper;
using SunemedicPRO_Inventarios.Server.DTOs;
using SunemedicPRO_Inventarios.Server.Entities;



namespace SunemedicPRO_Inventarios.Server.Utilidades
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
