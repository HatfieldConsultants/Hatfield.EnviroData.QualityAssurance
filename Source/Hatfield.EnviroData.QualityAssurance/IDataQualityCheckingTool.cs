using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hatfield.EnviroData.QualityAssurance
{    
    public interface IDataQualityCheckingTool
    {
        IDataQualityCheckingData DataQualityCheckingData { get; }
        IQualityCheckingResult Check(object data);
        IQualityCheckingResult Correct(object data);
        bool IsDataQualityChekcingDataSupport(IDataQualityCheckingData dataQualityCheckingData);
        bool IsDataSupport(object data);
    }
}
