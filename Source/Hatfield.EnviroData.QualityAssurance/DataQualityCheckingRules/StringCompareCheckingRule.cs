using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hatfield.EnviroData.QualityAssurance.DataQualityCheckingRules
{
    public class StringCompareCheckingRule : IDataQualityCheckingRule
    {
        public string ExpectedValue { get; set; }
        public bool IsCaseSensitive { get; set; }
        public string CorrectionValue { get; set; }

        public StringCompareCheckingRule(string expectedValue, bool isCaseSensitive, string correctionValue)
        {
            ExpectedValue = expectedValue;
            IsCaseSensitive = isCaseSensitive;
            CorrectionValue = correctionValue;
        }
    }
}
