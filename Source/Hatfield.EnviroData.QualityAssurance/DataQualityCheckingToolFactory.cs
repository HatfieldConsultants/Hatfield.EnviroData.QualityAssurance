using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hatfield.EnviroData.QualityAssurance
{
    public class DataQualityCheckingToolFactory : IDataQualityCheckingToolFactory
    {
        public IDataQualityCheckingTool GenerateDataQualityCheckingTool(DataQualityCheckingToolConfiguration toolConfiguration)
        {
            throw new NotImplementedException();
        }
    }
}
