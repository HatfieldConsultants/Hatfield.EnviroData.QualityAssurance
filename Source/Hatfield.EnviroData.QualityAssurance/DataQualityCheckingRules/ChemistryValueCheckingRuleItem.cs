using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hatfield.EnviroData.QualityAssurance.DataQualityCheckingRules
{
    public class ChemistryValueCheckingRuleItem
    {
        public int ActionId { get; set; }
        public double? CorrectionValue { get; set; }
    }
}
