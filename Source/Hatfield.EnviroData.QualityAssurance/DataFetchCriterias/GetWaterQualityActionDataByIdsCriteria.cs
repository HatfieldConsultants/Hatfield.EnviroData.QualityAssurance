using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Hatfield.EnviroData.WQDataProfile;
using Hatfield.EnviroData.QualityAssurance.DataQualityCheckingRules;

namespace Hatfield.EnviroData.QualityAssurance.DataFetchCriterias
{
    public class GetWaterQualityActionDataByIdsCriteria : IDataFetchCriteria
    {
        private IWQDataRepository _wqDataRepository;
        private ChemistryValueCheckingRule _manualCheckingRule;

        public GetWaterQualityActionDataByIdsCriteria(IWQDataRepository wqDataRepository, ChemistryValueCheckingRule manualCheckingRule)
        {
            _wqDataRepository = wqDataRepository;
            _manualCheckingRule = manualCheckingRule;
        }

        public IEnumerable<object> FetchData()
        {
            //fetch data by the action ids in the chekcing rule
            throw new NotImplementedException();
        }

        public string CriteriaDescription
        {
            //return description
            get { throw new NotImplementedException(); }
        }
    }
}
