using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hatfield.EnviroData.QualityAssurance.DataQualityCheckingRules
{
    public class ChemistryValueCheckingRule : IDataQualityCheckingRule
    {
        public List<ChemistryValueCheckingRuleItem> Items { get; set; }

        public ChemistryValueCheckingRule()
        {
            Items = new List<ChemistryValueCheckingRuleItem>();
        }
    }
}
