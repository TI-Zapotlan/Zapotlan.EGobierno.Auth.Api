using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zapotlan.EGobierno.Auth.Core.DTOs;
using Zapotlan.EGobierno.Auth.Core.Entities;

namespace Zapotlan.EGobierno.Auth.Infrastructure.Mappings
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile() 
        {
            CreateMap<Area, AreaDto>();
            CreateMap<AreaDto, Area>();

            CreateMap<Usuario, UsuarioDto>();
            CreateMap<UsuarioDto, Usuario>();
            CreateMap<Usuario, UsuarioListDto>();
            CreateMap<UsuarioListDto, Usuario>();
            CreateMap<Usuario, UsuarioDetailDto>()
                .ForMember(dto => dto.Grupos, o => o.Ignore())
                .ForMember(dto => dto.Derechos, o => o.Ignore());
            CreateMap<UsuarioDetailDto, Usuario>();
            CreateMap<Usuario, UsuarioUpdateDto>();
            CreateMap<UsuarioUpdateDto, Usuario>();
        }
    }
}
