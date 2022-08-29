using Fanex.Data;
using Fanex.Data.Repository;

namespace Fanex.Settlement.ScaleOut.Criteria
{
    public class GetDeploymentInfoCriteria : NonQueryCommand
    {
        public string SpName { get; set; }

        public bool IsWaiting { get; set; }

        public string WaitingTime { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        [SpParam(Ignored = true)]
        public string DbServer { get; set; }

        public override string GetSettingKey() => $"NAP_Set_SPDeploymentInfo_{DbServer}";

        public override bool IsValid() => !string.IsNullOrEmpty(SpName);
    }
}