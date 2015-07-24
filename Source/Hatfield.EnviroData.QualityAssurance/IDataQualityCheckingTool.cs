using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hatfield.EnviroData.QualityAssurance
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T">Data Quality Chekcing Data that would be used by the tool</typeparam>
    /// <typeparam name="L">Data that need to quality chekcing</typeparam>
    public interface IDataQualityCheckingTool<T, L> where T : IDataQualityCheckingData
    {
        T DataQualityCheckingData { get; }
        bool Check(L data);
        L Correct(L data);
    }
}
