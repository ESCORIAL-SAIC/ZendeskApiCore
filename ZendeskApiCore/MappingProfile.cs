using ZendeskApiCore.Models;
using AutoMapper;

namespace ZendeskApiCore
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ReclamoWebZendeskDto, ReclamoWebZendesk>();
            CreateMap<ItemReclamoWebZendeskDto, ItemReclamoWebZendesk>();
            CreateMap<Login, UserInfoDto>();
            CreateMap<Producto, ProductoDto>();
            CreateMap<Problema, ProblemaDto>()
                .ForMember(dest => dest.Rubro, opt => opt.Ignore());
            CreateMap<TrReclamo, TrReclamoDto>()
                .ForMember(dest => dest.Flag, opt => opt.Ignore());
        }
    }
}
