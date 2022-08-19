using Fanex.Data.Repository;

namespace Fanex.Settlement.ScaleOut.Criteria
{
    public class ValidateEventThirdPartyCriteria : NonQueryCommand
    {
        public int MatchId { get; set; }

        public int VendorId { get; set; }

        public string Action { get; set; }

        public string Mode { get; set; }

        public bool IsAuto { get; set; }

        public string SelectedResources { get; set; }

        public string SpName { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey() => "NAP_Admin_Settlement_3rdValidation_Event";

        public override bool IsValid() => MatchId > 0;
    }
}
