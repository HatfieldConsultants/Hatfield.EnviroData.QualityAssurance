using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Hatfield.EnviroData.Core;
using Hatfield.EnviroData.DataAcquisition.ESDAT.Converters;
using Hatfield.EnviroData.QualityAssurance.DataQualityCheckingRules;

namespace Hatfield.EnviroData.QualityAssurance.DataQualityCheckingTool
{
    public class SampleMatrixTypeCheckingTool : IDataQualityCheckingTool
    {
        private string validSampleMatrixResultFormat = "Sample Result: {0}'s sample matrix meets the expected value.";
        private string invalidSampleMatrixResultFormat = "Sample Result: {0}'s sample matrix value is {1}, the expected value is {2}. Need data correction.";

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

        public void Correct(object data, IDataQualityCheckingRule dataQualityCheckingRule)
        {
            throw new NotImplementedException();
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

        private IQualityCheckingResult IsSampleMatrixTypeDataMeetQualityCheckingRule(Hatfield.EnviroData.Core.Action sampleActionData, 
                                                                                     StringCompareCheckingRule rule)
        {
            var resultStringBuilder = new StringBuilder();
            var needCorrection = false;

            var sampleResults = from featureAction in sampleActionData.FeatureActions
                                from result in featureAction.Results
                                where featureAction.SamplingFeature.SamplingFeatureTypeCV == "Site"
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
