using Fanex.Data;
using Fanex.Data.Repository;

namespace Fanex.Settlement.ScaleOut.Criteria
{
    public class ValidateMoveBetDayCriteria : NonQueryCommand
    {
        public string MatchIds { get; set; }

        public string EventTable { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        [SpParam(Ignored = true)]
        public string DbServer { get; set; }

        public override string GetSettingKey()
        {
            return $"NAP_Admin_Settlement_CheckingMoveBetDay_{DbServer}";
        }

        public override bool IsValid()
        {
            return MatchIds.Length > 0;
        }
    }
}