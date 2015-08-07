using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Hatfield.EnviroData.Core;
using Hatfield.EnviroData.DataAcquisition.ESDAT.Converters;
using Hatfield.EnviroData.QualityAssurance.DataQualityCheckingRules;
using Hatfield.EnviroData.WQDataProfile;

namespace Hatfield.EnviroData.QualityAssurance.DataQualityCheckingTool
{
    public class ChemistryValueCheckingTool : IDataQualityCheckingTool
    {
        private string validManualQualityResultFormat = "Sample Result: {0}'s value meets the expected value.";
        private string invalidManualQualityResultFormat = "Sample Result: {0}'s value is {1}, the expected value is {2}. Need data correction.";

        private IDataVersioningHelper _dataVersioningHelper;
        private IRepository<CV_RelationshipType> _relationShipTypeCVRepository;

        public ChemistryValueCheckingTool(IDataVersioningHelper versioningHelper, IRepository<CV_RelationshipType> relationShipTypeCVRepository)
        {
            _dataVersioningHelper = versioningHelper;
            _relationShipTypeCVRepository = relationShipTypeCVRepository;
        }

        public IQualityCheckingResult Check(object data, IDataQualityCheckingRule rule)
        {
            var checkingResult = new ChemistryCheckingResult();

            if (!IsDataSupported(data))
            {
                return new QualityCheckingResult("Data is not supported by the Quality Checking Tool.", false, QualityCheckingResultLevel.Error);
            }
            else if (!IsDataQualityCheckingRuleSupported(rule))
            {
                return new QualityCheckingResult("Data Quality Checking Rule is not supported by the Quality Checking Tool.", false, QualityCheckingResultLevel.Error);
            }
            else
            {
                var castedData = (IEnumerable<Core.Action>)data;
                var castedRule = (ChemistryValueCheckingRule)rule;

                foreach (ChemistryValueCheckingRuleItem ruleItem in castedRule.Items)
                {
                    // Assume that only one action is present for each matching rule
                    var matchingAction = castedData.Where(x => x.ActionID.Equals(ruleItem.ActionId)).FirstOrDefault();

                    var latestVersionData = _dataVersioningHelper.GetLatestVersionActionData(matchingAction);

                    var qcResult = DoesValueMeetRuleItem(latestVersionData, ruleItem);

                    checkingResult.AddResult(qcResult);
                }
            }

            return checkingResult;
        }

        public IQualityCheckingResult Correct(object data, IDataQualityCheckingRule rule)
        {
            var checkResult = Check(data, rule);

            var chemistryCheckingResult = new ChemistryCheckingResult();
            chemistryCheckingResult.AddResult(checkResult);

            if (checkResult.NeedCorrection)
            {
                var castedData = (IEnumerable<Core.Action>)data;
                var castedRule = (ChemistryValueCheckingRule)rule;

                foreach (ChemistryValueCheckingRuleItem ruleItem in castedRule.Items)
                {
                    // Assume only one action is present for each matching rule
                    var matchingAction = castedData.Where(x => x.ActionID.Equals(ruleItem.ActionId)).FirstOrDefault();

                    var latestVersionData = _dataVersioningHelper.GetLatestVersionActionData(matchingAction);

                    var qcResult = GenerateCorrectChemistryDataNewVersion(latestVersionData, ruleItem);

                    chemistryCheckingResult.AddResult(qcResult);
                }
            }
            else if (checkResult.Level == QualityCheckingResultLevel.Info)
            {
                return new QualityCheckingResult("Sample data meets quality checking rule. No correction is needed.", false, QualityCheckingResultLevel.Info);
            }
            else
            {
                return new QualityCheckingResult("Sample data/Data checking rule is not valid. No correction could be applied.", false, QualityCheckingResultLevel.Error);
            }

            return chemistryCheckingResult;
        }

        public bool IsDataQualityCheckingRuleSupported(IDataQualityCheckingRule rule)
        {
            return rule is ChemistryValueCheckingRule;
        }

        /// <summary>
        /// Checks if data is of the supported data type and supported format
        /// </summary>
        /// <param name="data">Any object</param>
        /// <returns>True if object is an IEnumerable<Core.Action> where each Core.Action is a Chemistry action; false otherwise</returns>
        public bool IsDataSupported(object data)
        {
            if (!(data is IEnumerable<Core.Action>))
            {
                return false;
            }
            else
            {
                var castedData = (IEnumerable<Core.Action>)data;

                // Verify that each Action is a Chemistry action
                foreach (Core.Action action in castedData)
                {
                    if (action.ActionTypeCV != "Specimen analysis")
                    {
                        return false;
                    }
                    //var featureActions = action.FeatureActions;

                    //// Assume that Chemistry actions contain only 1 Feature action
                    //if (featureActions.Count() != 1)
                    //{
                    //    return false;
                    //}
                    //else
                    //{
                    //    var samplingFeature = featureActions.FirstOrDefault().SamplingFeature;

                    //    // TODO Move hard-coded "specimen" to static constants
                    //    if (samplingFeature.SamplingFeatureTypeCV != "Specimen")
                    //    {
                    //        return false;
                    //    }
                    //}
                }
            }

            return true;
        }

        private IQualityCheckingResult GenerateCorrectChemistryDataNewVersion(Core.Action action, ChemistryValueCheckingRuleItem ruleItem)
        {
            var newVersionOfData = CorrectChemistryData(action, ruleItem);

            var newRelationAction = new RelatedAction();

            newRelationAction.Action = action;
            newRelationAction.Action1 = newVersionOfData;
            newRelationAction.RelationshipTypeCV = QualityAssuranceConstants.SubVersionRelationCVType;
            newRelationAction.CV_RelationshipType = _relationShipTypeCVRepository.GetAll()
                .Where(x => x.Name == QualityAssuranceConstants.SubVersionRelationCVType).FirstOrDefault();

            action.RelatedActions.Add(newRelationAction);

            return new QualityCheckingResult("Create new version for correction data.", false, QualityCheckingResultLevel.Info);
        }

        private Core.Action CorrectChemistryData(Core.Action action, ChemistryValueCheckingRuleItem ruleItem)
        {
            var newVersionOfData = _dataVersioningHelper.CloneActionData(action);

            // Assume only one measurement result value
            var measurementresultValue = newVersionOfData.FeatureActions.FirstOrDefault().Results.FirstOrDefault().MeasurementResult.MeasurementResultValues.FirstOrDefault();

            measurementresultValue.DataValue = (double)ruleItem.CorrectionValue;

            return newVersionOfData;
        }

        private IQualityCheckingResult DoesValueMeetRuleItem(Core.Action action, ChemistryValueCheckingRuleItem ruleItem)
        {
            if (action == null || action.FeatureActions == null || !action.FeatureActions.Any())
            {
                return new QualityCheckingResult("No result found for the action.", false, QualityCheckingResultLevel.Info);
            }

            // Assume chemistry action contains only one result
            var result = action.FeatureActions.FirstOrDefault().Results.FirstOrDefault();

            if (result == null)
            {
                return new QualityCheckingResult("No result found for the action.", false, QualityCheckingResultLevel.Info);
            }

            var resultStringBuilder = new StringBuilder();
            var needCorrection = false;

            // Assume only one measurement result value
            var measurementResultValue = result.MeasurementResult.MeasurementResultValues.FirstOrDefault();

            if (measurementResultValue == null)
            {
                resultStringBuilder.AppendLine("Sample Result: " + ResultToString(result) + " does not contain data.");
            }
            else
            {
                if (measurementResultValue.DataValue == ruleItem.CorrectionValue)
                {
                    resultStringBuilder.AppendLine(
                        string.Format(validManualQualityResultFormat, ResultToString(result))
                    );
                }
                else
                {
                    resultStringBuilder.AppendLine(
                        string.Format(invalidManualQualityResultFormat,
                                      ResultToString(result),
                                      measurementResultValue.DataValue,
                                      ruleItem.CorrectionValue)
                    );

                    needCorrection = true;
                }
            }


            var qcResult = new QualityCheckingResult(resultStringBuilder.ToString(), needCorrection, QualityCheckingResultLevel.Info);

            return qcResult;
        }

        private string ResultToString(Result result)
        {
            var resultStringBuilder = new StringBuilder();
            resultStringBuilder.Append(result.ResultUUID.ToString());

            if (result.ResultDateTime.HasValue)
            {
                resultStringBuilder.Append(" on ");
                resultStringBuilder.Append(result.ResultDateTime.Value.ToString("MMM-dd-yyyy"));
            }

            return resultStringBuilder.ToString();
        }
    }
}
