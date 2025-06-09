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
        }
    }
}
