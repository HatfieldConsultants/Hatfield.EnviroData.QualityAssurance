﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hatfield.EnviroData.QualityAssurance
{
    public class DataQualityCheckingToolConfiguration
    {        
        public Type DataQualityCheckingToolType { get; set; }
        public IDataQualityCheckingRule DataQualityCheckingRule { get; set; }
    }
}
