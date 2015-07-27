using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;
using Moq;

using Hatfield.EnviroData.Core;
using Hatfield.EnviroData.QualityAssurance;
using Hatfield.EnviroData.QualityAssurance.DataQualityCheckingRules;
using Hatfield.EnviroData.QualityAssurance.DataQualityCheckingTool;

namespace Hatfield.EnviroData.QualityAssurance.Test.DataQualityCheckingTool
{
    [TestFixture]
    public class SampleMatrixTypeCheckingToolTest
    {
        [Test]
        public void DataNotSupportCheckTest()
        {
            var testTool = new SampleMatrixTypeCheckingTool();
            var testQualityCheckingRule = new StringCompareCheckingRule("test", false, "test");

            var dataNotSupportCheckResult = testTool.Check(1, testQualityCheckingRule);

            Assert.NotNull(dataNotSupportCheckResult);
            Assert.AreEqual("Data is not supported by the Quality Checking Tool.", dataNotSupportCheckResult.Message);
            Assert.False(dataNotSupportCheckResult.NeedCorrection);
            Assert.AreEqual(QualityCheckingResultLevel.Error, dataNotSupportCheckResult.Level);
        }

        [Test]
        public void RuleNotSupportCheckTest()
        {
            var testTool = new SampleMatrixTypeCheckingTool();
            var mockQualityCheckingRule = new Mock<IDataQualityCheckingRule>();

            var dataNotSupportCheckResult = testTool.Check(new Hatfield.EnviroData.Core.Action(), mockQualityCheckingRule.Object);

            Assert.NotNull(dataNotSupportCheckResult);
            Assert.AreEqual("Data Quality Checking Rule is not supported by the Quality Checking Tool.", dataNotSupportCheckResult.Message);
            Assert.False(dataNotSupportCheckResult.NeedCorrection);
            Assert.AreEqual(QualityCheckingResultLevel.Error, dataNotSupportCheckResult.Level);
        }

        [Test]
        public void ValidCheckTest()
        {
            var testActionData = new Hatfield.EnviroData.Core.Action
            {
                FeatureActions = new List<FeatureAction> { 
                    new FeatureAction{
                        SamplingFeature = new SamplingFeature{
                            SamplingFeatureTypeCV = "Site"
                        },
                        Results = new List<Result>{
                            new Result{
                                ResultUUID = new Guid("abdc02ff-4bc6-4b8c-8578-e3c8d3620cea"),
                                ResultDateTime = new DateTime(2015, 5, 1),
                                ResultExtensionPropertyValues = new List<ResultExtensionPropertyValue>{
                                    new ResultExtensionPropertyValue{
                                        ExtensionProperty = new ExtensionProperty{
                                            PropertyName = "Matrix Type"
                                        },
                                        PropertyValue = "Water"
                                    }
                                }
                            },
                            new Result{
                                ResultUUID = new Guid("abdc02ff-4bc6-4b8c-8578-e3c8d3620cea"),                                
                                ResultExtensionPropertyValues = new List<ResultExtensionPropertyValue>{
                                    new ResultExtensionPropertyValue{
                                        ExtensionProperty = new ExtensionProperty{
                                            PropertyName = "Matrix Type"
                                        },
                                        PropertyValue = "water"
                                    }
                                }
                            },
                            new Result{
                                ResultUUID = new Guid("abdc02ff-4bc6-4b8c-8578-e3c8d3620cea"),
                                ResultDateTime = new DateTime(2015, 5, 1),
                                ResultExtensionPropertyValues = new List<ResultExtensionPropertyValue>{
                                    new ResultExtensionPropertyValue{
                                        ExtensionProperty = new ExtensionProperty{
                                            PropertyName = "Matrix Type"
                                        },
                                        PropertyValue = "test type"
                                    }
                                }
                            },
                            new Result{
                                ResultUUID = new Guid("abdc02ff-4bc6-4b8c-8578-e3c8d3620cea"),
                                ResultDateTime = new DateTime(2015, 5, 1),
                                ResultExtensionPropertyValues = new List<ResultExtensionPropertyValue>{                                    
                                }
                            }
                        
                        }
                    }
                }
            };

            var testQualityCheckingRule = new StringCompareCheckingRule("Water", true, "test");
            var testTool = new SampleMatrixTypeCheckingTool();

            var qcResult = testTool.Check(testActionData, testQualityCheckingRule);

            var expextedMessageBuilder = new StringBuilder();
            expextedMessageBuilder.AppendLine("Sample Result: abdc02ff-4bc6-4b8c-8578-e3c8d3620cea on May-01-2015's sample matrix meets the expected value.");
            expextedMessageBuilder.AppendLine("Sample Result: abdc02ff-4bc6-4b8c-8578-e3c8d3620cea's sample matrix meets the expected value.");
            expextedMessageBuilder.AppendLine("Sample Result: abdc02ff-4bc6-4b8c-8578-e3c8d3620cea on May-01-2015's sample matrix value is test type, the expected value is Water. Need data correction.");
            expextedMessageBuilder.AppendLine("Sample Result: abdc02ff-4bc6-4b8c-8578-e3c8d3620cea on May-01-2015 does not contain sample matrix data.");

            Assert.AreEqual(expextedMessageBuilder.ToString(), qcResult.Message);
            Assert.True(qcResult.NeedCorrection);
            Assert.AreEqual(QualityCheckingResultLevel.Info, qcResult.Level);
        }

        [Test]
        public void IsDataQualityChekcingDataSupportTest()
        {
            var successTestQualityCheckingRule = new StringCompareCheckingRule("test", false, "test");
            var mockQualityCheckingRule = new Mock<IDataQualityCheckingRule>();

            var testTool = new SampleMatrixTypeCheckingTool();

            Assert.True(testTool.IsDataQualityChekcingRuleSupported(successTestQualityCheckingRule));
            Assert.False(testTool.IsDataQualityChekcingRuleSupported(mockQualityCheckingRule.Object));
        }

        [Test]
        public void IsDataSupportTest()
        {
            var testTool = new SampleMatrixTypeCheckingTool();
            var supportData = new Hatfield.EnviroData.Core.Action();

            Assert.True(testTool.IsDataSupport(supportData));
            Assert.False(testTool.IsDataSupport(1));
        }
    }
}
