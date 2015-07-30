using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;
using Moq;

using Hatfield.EnviroData.Core;
using Hatfield.EnviroData.WQDataProfile;
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
            var mockVersionHelper = new Mock<IDataVersioningHelper>();
            var mockRepository = new Mock<IRepository<CV_RelationshipType>>();

            var testTool = new SampleMatrixTypeCheckingTool(mockVersionHelper.Object, mockRepository.Object);
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
            var mockVersionHelper = new Mock<IDataVersioningHelper>();
            var mockRepository = new Mock<IRepository<CV_RelationshipType>>();

            var testTool = new SampleMatrixTypeCheckingTool(mockVersionHelper.Object, mockRepository.Object);
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
            var testActionData = CreateTestAction();
            var testQualityCheckingRule = new StringCompareCheckingRule("Water", false, "test");

            var mockVersionHelper = new Mock<IDataVersioningHelper>();
            mockVersionHelper.Setup(x => x.GetLatestVersionActionData(It.IsAny<Hatfield.EnviroData.Core.Action>())).Returns(testActionData);
            var mockRepository = new Mock<IRepository<CV_RelationshipType>>();

            var testTool = new SampleMatrixTypeCheckingTool(mockVersionHelper.Object, mockRepository.Object);
            
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

            var mockVersionHelper = new Mock<IDataVersioningHelper>();
            var mockRepository = new Mock<IRepository<CV_RelationshipType>>();

            var testTool = new SampleMatrixTypeCheckingTool(mockVersionHelper.Object, mockRepository.Object);

            Assert.True(testTool.IsDataQualityChekcingRuleSupported(successTestQualityCheckingRule));
            Assert.False(testTool.IsDataQualityChekcingRuleSupported(mockQualityCheckingRule.Object));
        }

        [Test]
        public void IsDataSupportTest()
        {
            var mockVersionHelper = new Mock<IDataVersioningHelper>();
            var mockRepository = new Mock<IRepository<CV_RelationshipType>>();

            var testTool = new SampleMatrixTypeCheckingTool(mockVersionHelper.Object, mockRepository.Object);
            var supportData = new Hatfield.EnviroData.Core.Action();

            Assert.True(testTool.IsDataSupport(supportData));
            Assert.False(testTool.IsDataSupport(1));
        }

        [Test]
        public void InvalidDataCorrectTest()
        {
            var mockVersionHelper = new Mock<IDataVersioningHelper>();
            var mockRepository = new Mock<IRepository<CV_RelationshipType>>();
            var mockQualityCheckingRule = new Mock<IDataQualityCheckingRule>();

            var testTool = new SampleMatrixTypeCheckingTool(mockVersionHelper.Object, mockRepository.Object);

            var correctResult = testTool.Correct(1, mockQualityCheckingRule.Object);

            Assert.NotNull(correctResult);
            Assert.AreEqual("Sample data/Data checking rule is not valid. No correction could be applied.", correctResult.Message);
            Assert.AreEqual(QualityCheckingResultLevel.Error, correctResult.Level);
            Assert.False(correctResult.NeedCorrection);
        }

        [Test]
        public void DataDoesNotNeedCorrectionTest()
        {
            var mockVersionHelper = new Mock<IDataVersioningHelper>();
            var mockRepository = new Mock<IRepository<CV_RelationshipType>>();
            
            var testQualityCheckingRule = new StringCompareCheckingRule("Water", true, "test");

            var testTool = new SampleMatrixTypeCheckingTool(mockVersionHelper.Object, mockRepository.Object);

            var correctResult = testTool.Correct(new Hatfield.EnviroData.Core.Action(), testQualityCheckingRule);

            Assert.NotNull(correctResult);
            Assert.AreEqual("Sample data meets quality checking rule. No correction is needed.", correctResult.Message);
            Assert.AreEqual(QualityCheckingResultLevel.Info, correctResult.Level);
            Assert.False(correctResult.NeedCorrection);
        }

        [Test]
        public void DataNeedCorrectionTest()
        {
            var testActionData = CreateTestAction();
            var testQualityCheckingRule = new StringCompareCheckingRule("Water", true, "test");

            var mockVersionHelper = new Mock<IDataVersioningHelper>();
            var mockRepository = new Mock<IRepository<CV_RelationshipType>>();
            mockRepository.Setup(x => x.GetAll())
                            .Returns(() => 
                                (new List<CV_RelationshipType> { 
                                    new CV_RelationshipType{
                                        Name = "Is new version of"
                                    }
                                })
                                .AsQueryable());

            var mockDefaultValueProvider = new Mock<IWQDefaultValueProvider>();
            mockDefaultValueProvider.Setup(x => x.ActionRelationshipTypeSubVersion).Returns("is new version of");

            var testTool = new SampleMatrixTypeCheckingTool(new DataVersioningHelper(mockDefaultValueProvider.Object), mockRepository.Object);

            var correctedResult = testTool.Correct(testActionData, testQualityCheckingRule);

            Assert.NotNull(correctedResult);
            Assert.AreEqual("Create new version for correction data.", correctedResult.Message);
            Assert.AreEqual(QualityCheckingResultLevel.Info, correctedResult.Level);
            Assert.False(correctedResult.NeedCorrection);

            var relationShips = testActionData.RelatedActions;
            Assert.AreEqual(1, relationShips.Count);
            Assert.AreEqual("Is new version of", relationShips.ElementAt(0).RelationshipTypeCV);
            Assert.AreEqual("Is new version of", relationShips.ElementAt(0).CV_RelationshipType.Name);

            var newVersionAction = relationShips.ElementAt(0).Action1;

            Assert.NotNull(newVersionAction);

            foreach(var featureAction in newVersionAction.FeatureActions)
            {
                foreach(var result in featureAction.Results)
                {
                    var extension = result.ResultExtensionPropertyValues.Where(x => x.ExtensionProperty.PropertyName == "Matrix Type").FirstOrDefault();

                    if(extension != null)
                    {
                        Assert.AreEqual("test", extension.PropertyValue);
                    }
                }
            }
        }

        private Hatfield.EnviroData.Core.Action CreateTestAction()
        {
            var testActionData = new Hatfield.EnviroData.Core.Action
            {
                FeatureActions = new List<FeatureAction> { 
                    new FeatureAction{
                        SamplingFeature = new SamplingFeature{
                            SamplingFeatureTypeCV = "Specimen"
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

            return testActionData;
        }
    }
}
