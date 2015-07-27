using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hatfield.EnviroData.QualityAssurance
{
    public interface IQualityCheckingResult
    {
        string Message { get; }
        bool NeedCorrection { get; }
        QualityCheckingResultLevel Level { get; }
    }

}
