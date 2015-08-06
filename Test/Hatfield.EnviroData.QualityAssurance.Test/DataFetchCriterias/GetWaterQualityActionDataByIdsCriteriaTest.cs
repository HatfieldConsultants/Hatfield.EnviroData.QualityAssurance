using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;
using Moq;

using Hatfield.EnviroData.Core;
using Hatfield.EnviroData.WQDataProfile;
using Hatfield.EnviroData.QualityAssurance.DataFetchCriterias;
using Hatfield.EnviroData.QualityAssurance.DataQualityCheckingRules;

namespace Hatfield.EnviroData.QualityAssurance.Test.DataFetchCriterias
{
    [TestFixture]
    public class GetWaterQualityActionDataByIdsCriteriaTest
    {
        const int Limit = 50;

        Mock<IWQDataRepository> _mockRepository;

        [SetUp]
        public void SetUp()
        {
            var actionList = new List<Core.Action>();
            for (int i = 0; i < Limit; i++)
            {
                var action = new Core.Action();
                action.ActionID = i;
                actionList.Add(action);
            }

            var mockRepository = new Mock<IWQDataRepository>();
            mockRepository.Setup(x => x.GetAllWQSampleDataActions()).Returns(actionList);
            _mockRepository = mockRepository;
        }

        [Test]
        public void NullArgumentConstructorTest()
        {
            Assert.Throws(typeof(ArgumentNullException), () => new GetWaterQualityActionDataByIdsCriteria(null, new Mock<ChemistryValueCheckingRule>().Object));
            Assert.Throws(typeof(ArgumentNullException), () => new GetWaterQualityActionDataByIdsCriteria(new Mock<IWQDataRepository>().Object, null));
            Assert.Throws(typeof(ArgumentNullException), () => new GetWaterQualityActionDataByIdsCriteria(null, null));
        }

        [Test]
        public void SuccessConstructorTest()
        {
            var testRule = new ChemistryValueCheckingRule();
            var ruleItemsCount = 0;

            // Construct the test rule
            for (int i = 0; i < Limit; i++)
            {
                // Create odd ids
                if ((i % 2) == 1)
                {
                    var ruleItem = new ChemistryValueCheckingRuleItem();
                    ruleItem.ActionId = i;
                    testRule.Items.Add(ruleItem);

                    ruleItemsCount++;
                }
            }

            var testCriteria = new GetWaterQualityActionDataByIdsCriteria(_mockRepository.Object, testRule);
            var testData = testCriteria.FetchData();

            // Rule item count and data count should match
            Assert.AreEqual(ruleItemsCount, testData.Count());

            int idMatchCount = 0;

            foreach (ChemistryValueCheckingRuleItem ruleItem in testRule.Items)
            {
                foreach (Core.Action action in testData)
                {
                    if (ruleItem.ActionId.Equals(action.ActionID))
                    {
                        idMatchCount++;
                    }
                }
            }

            // All rule ids should be accounted for
            Assert.AreEqual(idMatchCount, ruleItemsCount);
        }

        [Test]
        public void CriteriaDescription()
        {
            var testRule = new ChemistryValueCheckingRule();
            var testCriteria = new GetWaterQualityActionDataByIdsCriteria(_mockRepository.Object, testRule);

            Assert.AreEqual("GetWaterQualityActionDataByIdsCriteria: fetch water quality data with given ids from the database", testCriteria.CriteriaDescription);
        }
    }
}
