using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Hatfield.EnviroData.Core;
using Hatfield.EnviroData.WQDataProfile;
using Hatfield.EnviroData.QualityAssurance.DataQualityCheckingRules;

namespace Hatfield.EnviroData.QualityAssurance.DataFetchCriterias
{
    public class GetActionDataByIdsCriteria : IDataFetchCriteria
    {
        private IActionRepository _actionRepository;
        private ChemistryValueCheckingRule _chemistryValueCheckingRule;

        public GetActionDataByIdsCriteria(IActionRepository actionRepository, ChemistryValueCheckingRule manualCheckingRule)
        {
            if (actionRepository == null)
            {
                throw new ArgumentNullException("Action data repository is null. GetActionDataByIdsCriteria initial fail.");
            }
            else if (manualCheckingRule == null)
            {
                throw new ArgumentNullException("Checking rule is null. GetActionDataByIdsCriteria initial fail.");
            }
            else
            {
                _actionRepository = actionRepository;
                _chemistryValueCheckingRule = manualCheckingRule;
            }

        }

        public IEnumerable<object> FetchData()
        {
            var results = new List<Core.Action>();

            foreach (var ruleItem in _chemistryValueCheckingRule.Items)
            {
                var matchingAction = _actionRepository.GetActionById(ruleItem.ActionId);

                if (matchingAction != null)
                {
                    results.Add(matchingAction);
                }
            }

            return results;
        }

        public string CriteriaDescription
        {
            get {
                return "GetActionDataByIdsCriteria: fetch action data with given ids from the database";
            }
        }
    }
}
