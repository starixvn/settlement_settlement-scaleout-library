using Fanex.Data.Repository;

namespace Fanex.Settlement.ScaleOut.Criteria.BetBuilder
{
    public class ValidateBetBuilderCriteria : NonQueryCommand
    {
        public int MatchId { get; set; }

        public string Action { get; set; }

        public bool IsAuto { get; set; }

        public string SelectedMarket { get; set; }

        public int VendorId { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public string InvalidMarketId { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_Admin_Settlement_Betbuilder_Validation_Event";
        }

        public override bool IsValid()
        {
            return MatchId > 0;
        }
    }
}