using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Hatfield.EnviroData.Core;
using Hatfield.EnviroData.QualityAssurance.DataQualityCheckingRules;

namespace Hatfield.EnviroData.QualityAssurance.DataQualityCheckingTool
{
    public class SampleMatrixTypeCheckingTool : IDataQualityCheckingTool
    {

        public IQualityCheckingResult Check(object data, IDataQualityCheckingRule dataQualityCheckingRule)
        {
            throw new NotImplementedException();
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

        private bool IsSampleMatrixTypeDataMeetQualityCheckingRule()
        {
            throw new NotImplementedException();
        }
    }
}
