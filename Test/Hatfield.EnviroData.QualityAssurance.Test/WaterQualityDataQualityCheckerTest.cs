using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;
using Moq;

using Hatfield.EnviroData.WQDataProfile;
using Hatfield.EnviroData.QualityAssurance;

namespace Hatfield.EnviroData.QualityAssurance.Test
{
    [TestFixture]
    public class WaterQualityDataQualityCheckerTest
    {
        [Test]
        public void NoDataTest()
        {
            var noDataCriteria = new Mock<IDataFetchCriteria>();
            noDataCriteria.Setup(x => x.CriteriaDescription).Returns("test data fetch criteria");
            noDataCriteria.Setup(x => x.FetchData()).Returns(() => null);

            var testChainConfiguration = new DataQualityCheckingChainConfiguration();
            testChainConfiguration.DataFetchCriteria = noDataCriteria.Object;

            var factory = new Mock<IDataQualityCheckingToolFactory>();
            var repository = new Mock<IWQDataRepository>();

            var dataChecker = new WaterQualityDataQualityChecker(testChainConfiguration, factory.Object, repository.Object);

            var noDataQcResults = dataChecker.Check();

            Assert.NotNull(noDataQcResults);
            Assert.AreEqual(2, noDataQcResults.Count());
        }

        [Test]
        public void CompleteDataChecking()
        {
            var mockCriteria = new Mock<IDataFetchCriteria>();
            mockCriteria.Setup(x => x.CriteriaDescription).Returns("test data fetch criteria");
            mockCriteria.Setup(x => x.FetchData()).Returns(() => new List<Hatfield.EnviroData.Core.Action> { 
                new Hatfield.EnviroData.Core.Action()
            });

            var mockRule = new Mock<IDataQualityCheckingRule>();

            var testChainConfiguration = new DataQualityCheckingChainConfiguration();
            testChainConfiguration.DataFetchCriteria = mockCriteria.Object;
            testChainConfiguration.NeedToCorrectData = true;
            testChainConfiguration.ToolsConfiguration = new List<DataQualityCheckingToolConfiguration> { 
                new DataQualityCheckingToolConfiguration{
                    DataQualityCheckingToolType = typeof(int),
                    DataQualityCheckingRule = mockRule.Object
                }
            };

            var mockTool = new Mock<IDataQualityCheckingTool>();
            mockTool.Setup(x => x.Check(It.IsAny<object>(), It.IsAny<IDataQualityCheckingRule>())).Returns(
                () => new QualityCheckingResult("test qc result.", true, QualityCheckingResultLevel.Info)    
            );

            var factory = new Mock<IDataQualityCheckingToolFactory>();
            factory.Setup(x => x.GenerateDataQualityCheckingTool(It.IsAny<DataQualityCheckingToolConfiguration>())).Returns(() => mockTool.Object);
            var repository = new Mock<IWQDataRepository>();

            var dataChecker = new WaterQualityDataQualityChecker(testChainConfiguration, factory.Object, repository.Object);

            var qcResults = dataChecker.Check();

            Assert.NotNull(qcResults);
            Assert.AreEqual(3, qcResults.Count());

            Assert.AreEqual("Fetch data for quality chekcing by test data fetch criteria", qcResults.ElementAt(0).Message);
            Assert.AreEqual(QualityCheckingResultLevel.Info, qcResults.ElementAt(0).Level);
            Assert.False(qcResults.ElementAt(0).NeedCorrection);

            Assert.AreEqual("test qc result.", qcResults.ElementAt(1).Message);
            Assert.AreEqual(QualityCheckingResultLevel.Info, qcResults.ElementAt(1).Level);
            Assert.True(qcResults.ElementAt(1).NeedCorrection);

            Assert.AreEqual("Data correction updated.", qcResults.ElementAt(2).Message);
            Assert.AreEqual(QualityCheckingResultLevel.Info, qcResults.ElementAt(2).Level);
            Assert.False(qcResults.ElementAt(2).NeedCorrection);

        }
    }
}
