using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hatfield.EnviroData.QualityAssurance
{
    public class ChemistryCheckingResult : IQualityCheckingResult
    {        
        public bool NeedCorrection { get; set; }
        public QualityCheckingResultLevel Level { get; set; }
        public List<IQualityCheckingResult> QualityCheckingResults { get; private set; }

        public ChemistryCheckingResult()
        {
            QualityCheckingResults = new List<IQualityCheckingResult>();
            
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

            QualityCheckingResults.Add(result);
        }

        public string Message
        {
            get {
                var messageBuilder = new StringBuilder();
                foreach (var result in QualityCheckingResults)
                {
                    messageBuilder.AppendLine(result.Message);
                }

                return messageBuilder.ToString();
            }
        }
    }
}
