using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Hatfield.EnviroData.Core;

namespace Hatfield.EnviroData.QualityAssurance
{
    public interface IDataFetchCriteria
    {
        IEnumerable<Hatfield.EnviroData.Core.Action> FetchData();
        string CriteriaDescription { get; }
    }
}
