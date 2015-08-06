using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hatfield.EnviroData.QualityAssurance
{    
    public interface IDataQualityCheckingTool
    {
        IQualityCheckingResult Check(object data, IDataQualityCheckingRule dataQualityCheckingRule);
        IQualityCheckingResult Correct(object data, IDataQualityCheckingRule dataQualityCheckingRule);
        bool IsDataQualityCheckingRuleSupported(IDataQualityCheckingRule dataQualityCheckingRule);
        bool IsDataSupported(object data);
    }
}
