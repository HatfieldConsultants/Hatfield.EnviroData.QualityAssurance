using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hatfield.EnviroData.QualityAssurance.DataQualityCheckingData
{
    public class StringCompareCheckingData : IDataQualityCheckingData
    {
        public string ExpectedValue { get; set; }
        public bool IsCaseSensitive { get; set; }
        public string CorrectionValue { get; set; }

        public StringCompareCheckingData(string expectedValue, bool isCaseSensitive, string correctionValue)
        {
            ExpectedValue = expectedValue;
            IsCaseSensitive = isCaseSensitive;
            CorrectionValue = correctionValue;
        }
    }
}
