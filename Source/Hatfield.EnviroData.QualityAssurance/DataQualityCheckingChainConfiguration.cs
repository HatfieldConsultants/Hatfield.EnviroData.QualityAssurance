using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hatfield.EnviroData.QualityAssurance
{
    public class DataQualityCheckingChainConfiguration
    {
        public IDataFetchCriteria DataFetchCriteria { get; set; }
        public bool NeedToCorrectData { get; set; }
        public IEnumerable<DataQualityCheckingToolConfiguration> ToolsConfiguration { get; set; }
    }
}
