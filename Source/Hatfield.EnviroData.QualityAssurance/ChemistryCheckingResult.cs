using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hatfield.EnviroData.QualityAssurance
{
    public class ChemistryCheckingResult : IQualityCheckingResult
    {
        public string Message { get; set; }
        public bool NeedCorrection { get; set; }
        public QualityCheckingResultLevel Level { get; set; }
        public List<IQualityCheckingResult> IQualityCheckingResults { get; private set; }

        public ChemistryCheckingResult()
        {
            IQualityCheckingResults = new List<IQualityCheckingResult>();

            Message = "See the inner list for detailed results.";
            NeedCorrection = false;
            Level = QualityCheckingResultLevel.Info;
        }

        public void AddResult(IQualityCheckingResult result)
        {
            // Retain the most severe NeedCorrection status
            if (NeedCorrection == false)
            {
                NeedCorrection = result.NeedCorrection;
            }

            // Retain the most severe Level
            if (Level == QualityCheckingResultLevel.Info)
            {
                Level = result.Level;
            }

            IQualityCheckingResults.Add(result);
        }
    }
}
