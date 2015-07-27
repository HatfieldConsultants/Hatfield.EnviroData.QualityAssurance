using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Hatfield.EnviroData.QualityAssurance.DataQualityCheckingTool;

namespace Hatfield.EnviroData.QualityAssurance
{
    public class DataQualityCheckingToolFactory : IDataQualityCheckingToolFactory
    {
        public IDataQualityCheckingTool GenerateDataQualityCheckingTool(DataQualityCheckingToolConfiguration toolConfiguration)
        {
            if (toolConfiguration.DataQualityCheckingToolType == typeof(SampleMatrixTypeCheckingTool))
            {
                return new SampleMatrixTypeCheckingTool();
            }
            else
            {
                throw new NotImplementedException();
            }
            
        }
    }
}
