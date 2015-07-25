using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Hatfield.EnviroData.QualityAssurance.DataQualityCheckingData;

namespace Hatfield.EnviroData.QualityAssurance.DataQualityCheckingTool
{
    public class SampleMatrixTypeCheckingTool : IDataQualityCheckingTool
    {
        public IDataQualityCheckingData DataQualityCheckingData
        {
            get { throw new NotImplementedException(); }
        }

        public IQualityCheckingResult Check(object data)
        {
            throw new NotImplementedException();
        }

        public void Correct(object data)
        {
            throw new NotImplementedException();
        }

        public bool IsDataQualityChekcingDataSupport(IDataQualityCheckingData dataQualityCheckingData)
        {
            throw new NotImplementedException();
        }

        public bool IsDataSupport(object data)
        {
            throw new NotImplementedException();
        }
    }
}
