
using AutoMapper;
using APIServer.Aplication.GraphQL.DTO;
using APIServer.Domain.Core.Models.WebHooks;

namespace APIServer.Aplication.Mapping
{

    public class WebHookRecords_Mapping_Profile : Profile
    {
        public WebHookRecords_Mapping_Profile()
        {

            CreateMap<WebHookRecord, GQL_WebHookRecord>()
                .IncludeAllDerived()
                .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.ID))
                .ForMember(dest => dest.WebHookID, opt => opt.MapFrom(src => src.WebHookID))
                .ForMember(dest => dest.WebHookSystemID, opt => opt.MapFrom(src => src.WebHookID))
                .ForMember(dest => dest.StatusCode, opt => opt.MapFrom(src => src.StatusCode))
                .ForMember(dest => dest.ResponseBody, opt => opt.MapFrom(src => src.ResponseBody))
                .ForMember(dest => dest.RequestBody, opt => opt.MapFrom(src => src.RequestBody))
                .ForMember(dest => dest.RequestHeaders, opt => opt.MapFrom(src => src.RequestHeaders))
                .ForMember(dest => dest.Guid, opt => opt.MapFrom(src => src.Guid))
                .ForMember(dest => dest.Result, opt => opt.MapFrom(src => src.Result))
                .ForMember(dest => dest.TriggerType, opt => opt.MapFrom(src => src.HookType))
                .ForMember(dest => dest.Exception, opt => opt.MapFrom(src => src.Exception))
                .ForMember(dest => dest.Timestamp, opt => opt.MapFrom(src => src.Timestamp))
                .ForMember(dest => dest.WebHook, opt => opt.Ignore())
                .ReverseMap();
        }
    }
}


