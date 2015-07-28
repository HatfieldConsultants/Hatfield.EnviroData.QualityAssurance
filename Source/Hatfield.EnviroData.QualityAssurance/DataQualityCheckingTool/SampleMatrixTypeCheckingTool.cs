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
    public class SampleMatrixTypeCheckingTool : IDataQualityCheckingTool
    {
        private string validSampleMatrixResultFormat = "Sample Result: {0}'s sample matrix meets the expected value.";
        private string invalidSampleMatrixResultFormat = "Sample Result: {0}'s sample matrix value is {1}, the expected value is {2}. Need data correction.";

        private IDataVersioningHelper _dataVersioningHelper;
        private IRepository<CV_RelationshipType> _relationShipTypeCVRepository;

        public SampleMatrixTypeCheckingTool(IDataVersioningHelper versioningHelper, IRepository<CV_RelationshipType> relationShipTypeCVRepository)
        {
            _dataVersioningHelper = versioningHelper;
            _relationShipTypeCVRepository = relationShipTypeCVRepository;
        }

        public IQualityCheckingResult Check(object data, IDataQualityCheckingRule dataQualityCheckingRule)
        {
            if (!IsDataSupport(data))
            {
                return new QualityCheckingResult("Data is not supported by the Quality Checking Tool.", 
                                                 false, 
                                                 QualityCheckingResultLevel.Error);
            }
            else if (!IsDataQualityChekcingRuleSupported(dataQualityCheckingRule))
            {
                return new QualityCheckingResult("Data Quality Checking Rule is not supported by the Quality Checking Tool.", 
                                                 false, 
                                                 QualityCheckingResultLevel.Error);
            }
            else
            {
                var castedData = (Hatfield.EnviroData.Core.Action)data;
                var castedRule = (StringCompareCheckingRule)dataQualityCheckingRule;

                return IsSampleMatrixTypeDataMeetQualityCheckingRule(castedData, castedRule);
            }
        }

        public IQualityCheckingResult Correct(object data, IDataQualityCheckingRule dataQualityCheckingRule)
        {
            var checkResult = this.Check(data, dataQualityCheckingRule);

            if (checkResult.NeedCorrection)
            {
                var castedData = (Hatfield.EnviroData.Core.Action)data;
                var castedRule = (StringCompareCheckingRule)dataQualityCheckingRule;

                return GenerateCorrectSampleMatrixTypeDataNewVersion(castedData, castedRule);
            }
            else if(checkResult.Level == QualityCheckingResultLevel.Info)
            {
                return new QualityCheckingResult("Sample data meets quality checking rule. No correction is needed.", false, QualityCheckingResultLevel.Info);
            }
            else
            {
                return new QualityCheckingResult("Sample data/Data checking rule is not valid. No correction could be applied.", false, QualityCheckingResultLevel.Error);
            }
        }

        public bool IsDataQualityChekcingRuleSupported(IDataQualityCheckingRule dataQualityCheckingRule)
        {
            var isSupported = dataQualityCheckingRule is StringCompareCheckingRule;
            return isSupported;
        }

        public bool IsDataSupport(object data)
        {
            var isSupported = data is Hatfield.EnviroData.Core.Action;
            return isSupported;
        }

        private IQualityCheckingResult GenerateCorrectSampleMatrixTypeDataNewVersion(Hatfield.EnviroData.Core.Action sampleActionData,
                                                                                     StringCompareCheckingRule rule)
        {
            var newVersionOfData = CorrectSampleMatrixTypeData(sampleActionData, rule);


            var relationActionOfNewVersion = new RelatedAction();
            relationActionOfNewVersion.Action = sampleActionData;
            relationActionOfNewVersion.Action1 = newVersionOfData;
            relationActionOfNewVersion.RelationshipTypeCV = QualityAssuranceConstants.SubVersionRelationCVType;
            var relationShipCV = _relationShipTypeCVRepository.GetAll()
                                    .Where(x => x.Name == QualityAssuranceConstants.SubVersionRelationCVType)
                                    .FirstOrDefault();
            relationActionOfNewVersion.CV_RelationshipType = relationShipCV;

            sampleActionData.RelatedActions.Add(relationActionOfNewVersion);

            return new QualityCheckingResult("Create new version for correction data.", false, QualityCheckingResultLevel.Info);
        }

        private Hatfield.EnviroData.Core.Action CorrectSampleMatrixTypeData(Hatfield.EnviroData.Core.Action sampleActionData,
                                                                            StringCompareCheckingRule rule)
        {
            var newVersionOfData = _dataVersioningHelper.CloneActionData(sampleActionData);

            foreach (var featureAction in newVersionOfData.FeatureActions)
            {
                foreach(var result in featureAction.Results)
                {
                    foreach(var extension in result.ResultExtensionPropertyValues)
                    {
                        if (extension.ExtensionProperty.PropertyName == ESDATSampleCollectionConstants.ResultExtensionPropertyValueKeyMatrixType)
                        {
                            extension.PropertyValue = rule.CorrectionValue;
                        }
                    }
                }
            }

            return newVersionOfData;
        }

        private IQualityCheckingResult IsSampleMatrixTypeDataMeetQualityCheckingRule(Hatfield.EnviroData.Core.Action sampleActionData, 
                                                                                     StringCompareCheckingRule rule)
        {
            var resultStringBuilder = new StringBuilder();
            var needCorrection = false;

            var sampleResults = from featureAction in sampleActionData.FeatureActions
                                from result in featureAction.Results
                                where featureAction.SamplingFeature.SamplingFeatureTypeCV == QualityAssuranceConstants.SiteSampleFeatureTypeCV
                                select result;

            foreach(var result in sampleResults)
            {
                var sampleMatrixExtension = result.ResultExtensionPropertyValues
                                                    .Where(x => x.ExtensionProperty.PropertyName == 
                                                           ESDATSampleCollectionConstants.ResultExtensionPropertyValueKeyMatrixType)
                                                    .FirstOrDefault();

                if (sampleMatrixExtension == null)
                {
                    resultStringBuilder.AppendLine("Sample Result: " + ResultToString(result) + " does not contain sample matrix data.");
                }
                else
                {
                    var stringCompareOption = rule.IsCaseSensitive ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;

                    if (string.Equals(sampleMatrixExtension.PropertyValue, rule.ExpectedValue, stringCompareOption))
                    {
                        resultStringBuilder.AppendLine(
                            string.Format(validSampleMatrixResultFormat, ResultToString(result))
                        );
                    }
                    else
                    {
                        resultStringBuilder.AppendLine(
                            string.Format(invalidSampleMatrixResultFormat, 
                                          ResultToString(result), 
                                          sampleMatrixExtension.PropertyValue, 
                                          rule.ExpectedValue)
                        );
                        needCorrection = true;
                    }
                }
            }

            var qcResult = new QualityCheckingResult(resultStringBuilder.ToString(), needCorrection, QualityCheckingResultLevel.Info);
            return qcResult;
        }

        private string ResultToString(Result result)
        {
            var resultStringBuilder = new StringBuilder();
            resultStringBuilder.Append(result.ResultUUID.ToString());

            if(result.ResultDateTime.HasValue)
            {
                resultStringBuilder.Append(" on ");
                resultStringBuilder.Append(result.ResultDateTime.Value.ToString("MMM-dd-yyyy"));
            }

            return resultStringBuilder.ToString();
        }
    }
}
