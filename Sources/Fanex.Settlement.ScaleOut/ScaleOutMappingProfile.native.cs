using AutoMapper;
using Fanex.Settlement.ScaleOut.Criteria.BetBuilder;
using Fanex.Settlement.ScaleOut.Criteria.FastMarket;
using Fanex.Settlement.ScaleOut.Criteria.HappyFive;
using Fanex.Settlement.ScaleOut.Criteria.Normal;
using Fanex.Settlement.ScaleOut.Criteria.Update;
using Fanex.Settlement.ScaleOut.Request;

namespace Fanex.Settlement.ScaleOut
{
    public class ScaleOutMappingProfile : Profile
    {
        public ScaleOutMappingProfile()
        {
            CreateMap<EventTicketRequest, ValidateEventCriteria>();
            CreateMap<EventTicketRequest, ValidateTicketCriteria>();
            CreateMap<EventTicketFastMarketRequest, ValidationEventFastMarketCriteria>();
            CreateMap<EventTicketFastMarketRequest, ValidationTicketFastMarketCriteria>();
            CreateMap<UpdateMatchInfoRequest, UpdateMatchInfoCriteria>();
            CreateMap<BetBuilderRequest, ValidateBetBuilderCriteria>();
            CreateMap<UpdateMatchInfo3rdRequest, UpdateMatchInfo3rdCriteria>();
        }
    }
}