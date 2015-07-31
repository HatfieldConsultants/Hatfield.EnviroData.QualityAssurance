using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hatfield.EnviroData.QualityAssurance.DataQualityCheckingRules
{
    public class ManualCheckingRule : IDataQualityCheckingRule
    {
        public IEnumerable<ManulaCheckingRuleItem> Items { get; set; }
    }
}
