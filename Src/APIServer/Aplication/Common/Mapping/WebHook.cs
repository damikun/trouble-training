using AutoMapper;
using APIServer.Aplication.GraphQL.DTO;
using APIServer.Domain.Core.Models.WebHooks;

namespace APIServer.Aplication.Mapping
{

    public class WebHook_Mapping_Profile : Profile
    {
        public WebHook_Mapping_Profile()
        {

            CreateMap<WebHook, GQL_WebHook>()
                .IncludeAllDerived()
                .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.ID))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
                .ForMember(dest => dest.LastTrigger, opt => opt.MapFrom(src => src.LastTrigger))
                .ForMember(dest => dest.ListeningEvents, opt => opt.MapFrom(src => src.HookEvents))
                .ForMember(dest => dest.ContentType, opt => opt.MapFrom(src => src.ContentType))
                .ForMember(dest => dest.WebHookUrl, opt => opt.MapFrom(src => src.WebHookUrl))
                .ForMember(dest => dest.Records, opt => opt.Ignore())
                .ReverseMap();
        }
    }
}