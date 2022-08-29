using Fanex.Data.Repository;
using Fanex.Data;
using System;

namespace Fanex.Settlement.ScaleOut.Criteria
{
    public class CalculateCriteria : NonQueryCommand
    {
        [SpParam(Ignored = true)]
        public string DbServer { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
        {
            throw new NotImplementedException();
        }

        public override bool IsValid()
        {
            throw new NotImplementedException();
        }
    }
}