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
        private ChemistryValueCheckingRule _chemistryValueCheckingRule;

        public GetWaterQualityActionDataByIdsCriteria(IWQDataRepository wqDataRepository, ChemistryValueCheckingRule manualCheckingRule)
        {
            if (wqDataRepository == null)
            {
                throw new ArgumentNullException("Water quality data repository is null. GetAllWaterQualityDataCriteria initial fail.");
            }
            else if (manualCheckingRule == null)
            {
                throw new ArgumentNullException("Checking rule is null. GetAllWaterQualityDataCriteria initial fail.");
            }
            else
            {
                _wqDataRepository = wqDataRepository;
                _chemistryValueCheckingRule = manualCheckingRule;
            }

        }

        public IEnumerable<object> FetchData()
        {
            var allWQSampleDataActions = _wqDataRepository.GetAllWQSampleDataActions();
            var result = new List<Core.Action>();

            foreach (ChemistryValueCheckingRuleItem ruleItem in _chemistryValueCheckingRule.Items)
            {
                var matchingAction = allWQSampleDataActions.Where(x => x.ActionID.Equals(ruleItem.ActionId)).FirstOrDefault();

                if (matchingAction != null)
                {
                    result.Add(matchingAction);
                }
            }

            return result;
        }

        public string CriteriaDescription
        {
            get
            {
                return "GetWaterQualityActionDataByIdsCriteria: fetch water quality data with given ids from the database";
            }
        }
    }
}
