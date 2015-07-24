using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hatfield.EnviroData.QualityAssurance
{
    public interface IDataQualityCheckingToolFactory
    {
        IDataQualityCheckingTool GenerateDataQualityCheckingTool(DataQualityCheckingToolConfiguration toolConfiguration);
    }
}
