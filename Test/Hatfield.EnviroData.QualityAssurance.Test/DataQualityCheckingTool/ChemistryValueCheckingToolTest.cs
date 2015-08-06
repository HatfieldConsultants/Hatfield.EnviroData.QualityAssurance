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
    public class ChemistryValueCheckingToolTest
    {
        private const int ActionId = 123;
        private const double MeasurementValue = 101;

        private object supportedData;
        private ChemistryValueCheckingTool testTool;
        private Mock<ChemistryValueCheckingRule> mockChemistryValueCheckingRule;

        [SetUp]
        public void Setup()
        {
            var measurementResultValue = new MeasurementResultValue();
            measurementResultValue.DataValue = MeasurementValue;
            var measurementResult = new MeasurementResult();
            measurementResult.MeasurementResultValues.Add(measurementResultValue);
            var result = new Result();
            result.ResultDateTime = DateTime.Today;
            result.MeasurementResult = measurementResult;
            var samplingFeature = new SamplingFeature();
            samplingFeature.SamplingFeatureTypeCV = "Specimen";
            var featureAction = new FeatureAction();
            featureAction.SamplingFeature = samplingFeature;
            featureAction.Results.Add(result);
            var action = new Core.Action();
            action.ActionID = ActionId;
            action.FeatureActions.Add(featureAction);
            var actions = new List<Core.Action>();
            actions.Add(action);
            supportedData = actions;

            var mockVersionHelper = new Mock<IDataVersioningHelper>();
            mockVersionHelper.Setup(x => x.GetLatestVersionActionData(It.IsAny<Hatfield.EnviroData.Core.Action>())).Returns(action);
            mockVersionHelper.Setup(x => x.CloneActionData(It.IsAny<Hatfield.EnviroData.Core.Action>())).Returns(cloneAction(action));

            var mockRepository = new Mock<IRepository<CV_RelationshipType>>();
            testTool = new ChemistryValueCheckingTool(mockVersionHelper.Object, mockRepository.Object);

            mockChemistryValueCheckingRule = new Mock<ChemistryValueCheckingRule>();
        }

        [Test]
        public void DataSupportedCheckTest()
        {
            var testResult = testTool.Check(supportedData, mockChemistryValueCheckingRule.Object);

            Assert.NotNull(testResult);
            Assert.AreEqual("See the inner list for detailed results.", testResult.Message);
            Assert.IsFalse(testResult.NeedCorrection);
            Assert.AreEqual(QualityCheckingResultLevel.Info, testResult.Level);
        }

        [Test]
        public void DataNotSupportedCheckTest()
        {
            var dataNotSupportCheckResult = testTool.Check(1, mockChemistryValueCheckingRule.Object);

            Assert.NotNull(dataNotSupportCheckResult);
            Assert.AreEqual("Data is not supported by the Quality Checking Tool.", dataNotSupportCheckResult.Message);
            Assert.False(dataNotSupportCheckResult.NeedCorrection);
            Assert.AreEqual(QualityCheckingResultLevel.Error, dataNotSupportCheckResult.Level);
        }

        [Test]
        public void RuleNotSupportedCheckTest()
        {
            var unsupportedRule = new Mock<IDataQualityCheckingRule>();

            var testResult = testTool.Check(supportedData, unsupportedRule.Object);

            Assert.NotNull(testResult);
            Assert.AreEqual("Data Quality Checking Rule is not supported by the Quality Checking Tool.", testResult.Message);
            Assert.False(testResult.NeedCorrection);
            Assert.AreEqual(QualityCheckingResultLevel.Error, testResult.Level);
        }

        [Test]
        public void DoesNotNeedCorrectionCheckTest()
        {
            var rule = new ChemistryValueCheckingRule();
            var ruleItem = new ChemistryValueCheckingRuleItem();
            ruleItem.CorrectionValue = MeasurementValue;
            ruleItem.ActionId = ActionId;
            rule.Items.Add(ruleItem);

            var qcResult = testTool.Check(supportedData, rule);

            Assert.IsFalse(qcResult.NeedCorrection);
            Assert.AreEqual(QualityCheckingResultLevel.Info, qcResult.Level);
        }

        [Test]
        public void NeedsCorrectionCheckTest()
        {
            var rule = new ChemistryValueCheckingRule();
            var ruleItem = new ChemistryValueCheckingRuleItem();
            ruleItem.CorrectionValue = MeasurementValue + 1;
            ruleItem.ActionId = ActionId;
            rule.Items.Add(ruleItem);

            var qcResult = testTool.Check(supportedData, rule);

            Assert.IsTrue(qcResult.NeedCorrection);
            Assert.AreEqual(QualityCheckingResultLevel.Info, qcResult.Level);
        }

        [Test]
        public void IsDataSupportedTest()
        {
            Assert.True(testTool.IsDataSupported(supportedData));
            Assert.False(testTool.IsDataSupported(1));
        }

        [Test]
        public void InvalidDataCorrectTest()
        {
            var correctResult = testTool.Correct(1, mockChemistryValueCheckingRule.Object);

            Assert.NotNull(correctResult);
            Assert.AreEqual("Sample data/Data checking rule is not valid. No correction could be applied.", correctResult.Message);
            Assert.AreEqual(QualityCheckingResultLevel.Error, correctResult.Level);
            Assert.False(correctResult.NeedCorrection);
        }

        [Test]
        public void DataDoesNotNeedCorrectionTest()
        {
            var rule = new ChemistryValueCheckingRule();
            var ruleItem = new ChemistryValueCheckingRuleItem();
            ruleItem.CorrectionValue = MeasurementValue;
            ruleItem.ActionId = ActionId;
            rule.Items.Add(ruleItem);

            var qcResult = testTool.Correct(supportedData, rule);

            Assert.NotNull(qcResult);
            Assert.AreEqual("Sample data meets quality checking rule. No correction is needed.", qcResult.Message);
            Assert.AreEqual(QualityCheckingResultLevel.Info, qcResult.Level);
            Assert.False(qcResult.NeedCorrection);
        }

        [Test]
        public void DataNeedsCorrectionTest()
        {
            var rule = new ChemistryValueCheckingRule();
            var ruleItem = new ChemistryValueCheckingRuleItem();
            var correctionValue = MeasurementValue + 1;
            ruleItem.CorrectionValue = correctionValue;
            ruleItem.ActionId = ActionId;
            rule.Items.Add(ruleItem);

            var qcResult = testTool.Correct(supportedData, rule);
            var castedData = (IEnumerable<Core.Action>)supportedData;

            Assert.NotNull(qcResult);
            Assert.AreEqual("See the inner list for detailed results.", qcResult.Message);
            Assert.AreEqual(QualityCheckingResultLevel.Info, qcResult.Level);
            Assert.True(qcResult.NeedCorrection);
            Assert.AreEqual(correctionValue, castedData.First().RelatedActions.First().
                Action1.FeatureActions.First().Results.First().MeasurementResult.MeasurementResultValues.First().DataValue);
        }

        private Core.Action cloneAction(Core.Action action)
        {
            var measurementResultValueClone = new MeasurementResultValue();
            measurementResultValueClone.DataValue = action.FeatureActions.FirstOrDefault().Results.FirstOrDefault()
                .MeasurementResult.MeasurementResultValues.FirstOrDefault().DataValue;
            var measurementResultClone = new MeasurementResult();
            measurementResultClone.MeasurementResultValues.Add(measurementResultValueClone);
            var resultClone = new Result();
            resultClone.ResultDateTime = action.FeatureActions.FirstOrDefault().Results.FirstOrDefault().ResultDateTime;
            resultClone.MeasurementResult = measurementResultClone;
            var samplingFeatureClone = new SamplingFeature();
            samplingFeatureClone.SamplingFeatureTypeCV = action.FeatureActions.FirstOrDefault().SamplingFeature.SamplingFeatureTypeCV;
            var featureActionClone = new FeatureAction();
            featureActionClone.SamplingFeature = samplingFeatureClone;
            featureActionClone.Results.Add(resultClone);
            var actionClone = new Core.Action();
            actionClone.FeatureActions.Add(featureActionClone);

            return actionClone;
        }
    }
}
