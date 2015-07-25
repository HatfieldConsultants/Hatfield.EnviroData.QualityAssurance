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
