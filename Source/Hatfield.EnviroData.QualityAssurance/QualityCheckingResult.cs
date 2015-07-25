using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hatfield.EnviroData.QualityAssurance
{
    public class QualityCheckingResult : IQualityCheckingResult
    {
        public string Message { get; set; }
        public bool NeedCorrection { get; set; }

        public QualityCheckingResult(string message, bool needCorrection)
        {
            Message = message;
            NeedCorrection = needCorrection;
        }
    }
}
