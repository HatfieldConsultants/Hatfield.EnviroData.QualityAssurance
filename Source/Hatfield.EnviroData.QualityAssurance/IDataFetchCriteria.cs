using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Hatfield.EnviroData.Core;

namespace Hatfield.EnviroData.QualityAssurance
{
    public interface IDataFetchCriteria
    {
        IEnumerable<object> FetchData();
        string CriteriaDescription { get; }
    }
}
