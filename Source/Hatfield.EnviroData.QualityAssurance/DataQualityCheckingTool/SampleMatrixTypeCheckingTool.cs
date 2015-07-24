using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Hatfield.EnviroData.QualityAssurance.DataQualityCheckingData;

namespace Hatfield.EnviroData.QualityAssurance.DataQualityCheckingTool
{
    public class SampleMatrixTypeCheckingTool : IDataQualityCheckingTool<StringCompareCheckingData, Hatfield.EnviroData.Core.Action>
    {
        public StringCompareCheckingData DataQualityCheckingData
        {
            get { throw new NotImplementedException(); }
        }

        public bool Check(Core.Action data)
        {
            throw new NotImplementedException();
        }

        public Core.Action Correct(Core.Action data)
        {
            throw new NotImplementedException();
        }
    }
}
