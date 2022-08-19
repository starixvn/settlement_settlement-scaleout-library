using AutoMapper;
using Fanex.Settlement.ScaleOut.Criteria;
using Fanex.Settlement.ScaleOut.Request;

namespace Fanex.Settlement.ScaleOut
{
    public class ScaleOutMappingProfile : Profile
    {
        public ScaleOutMappingProfile (){
            CreateMap<EventTicketRequest, ValidateEventCriteria>();
            CreateMap<EventTicketRequest, ValidateTicketCriteria>();
            CreateMap<EventTicketFastMarketRequest, ValidationEventFastMarketCriteria>();
            CreateMap<EventTicketFastMarketRequest, ValidationTicketFastMarketCriteria>();
            CreateMap<UpdateMatchInfoRequest, UpdateMatchInfoCriteria>();

            CreateMap<EventTicketThirdPartyRequest, ValidateEventThirdPartyCriteria>();
            CreateMap<EventTicketThirdPartyRequest, ValidateTicketThirdPartyCriteria>();
        }
    }
}
