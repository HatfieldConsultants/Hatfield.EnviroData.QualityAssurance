using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Hatfield.EnviroData.Core;
using Hatfield.EnviroData.DataAcquisition.ESDAT.Converters;
using Hatfield.EnviroData.QualityAssurance.DataQualityCheckingRules;

namespace Hatfield.EnviroData.QualityAssurance.DataQualityCheckingTool
{
    public class SampleMatrixTypeCheckingTool : IDataQualityCheckingTool
    {

        public IQualityCheckingResult Check(object data, IDataQualityCheckingRule dataQualityCheckingRule)
        {
            if (IsDataSupport(data))
            {
                return new QualityCheckingResult("Data is not supported by the Quality Checking Tool.", false, QualityCheckingResultLevel.Error);
            }
            else if (IsDataQualityChekcingRuleSupported(dataQualityCheckingRule))
            {
                return new QualityCheckingResult("Data Quality Checking Rule is not supported by the Quality Checking Tool.", false, QualityCheckingResultLevel.Error);
            }
            else
            {
                var castedData = (Hatfield.EnviroData.Core.Action)data;
                var castedRule = (StringCompareCheckingRule)dataQualityCheckingRule;

                return IsSampleMatrixTypeDataMeetQualityCheckingRule(castedData, castedRule);
            }
        }

        public void Correct(object data, IDataQualityCheckingRule dataQualityCheckingRule)
        {
            throw new NotImplementedException();
        }

        public bool IsDataQualityChekcingRuleSupported(IDataQualityCheckingRule dataQualityCheckingRule)
        {
            return dataQualityCheckingRule is StringCompareCheckingRule;
        }

        public bool IsDataSupport(object data)
        {
            return data is Hatfield.EnviroData.Core.Action;
        }

        private IQualityCheckingResult IsSampleMatrixTypeDataMeetQualityCheckingRule(Hatfield.EnviroData.Core.Action sampleActionData, StringCompareCheckingRule rule)
        {
            
            throw new NotImplementedException();
        }
    }
}
