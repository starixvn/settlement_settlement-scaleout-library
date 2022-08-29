using System;
using Fanex.Data;
using Fanex.Data.Repository;

namespace Fanex.Settlement.ScaleOut.Criteria.FastMarket
{
    public class ValidationTicketFastMarketCriteria : NonQueryCommand
    {
        public int MatchId { get; set; }

        public string Action { get; set; }

        public bool IsAuto { get; set; }

        public string Results { get; set; }

        public bool IsSettleDetermined { get; set; }

        [SpParam(Ignored = true)]
        public string DbServer { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey() => $"NAP_Admin_Settlement_ValidationFM_Ticket_{DbServer}";

        public override bool IsValid() => MatchId > 0;
    }
}