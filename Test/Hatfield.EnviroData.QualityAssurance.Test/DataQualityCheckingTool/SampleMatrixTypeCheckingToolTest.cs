using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;
using Moq;

using Hatfield.EnviroData.Core;
using Hatfield.EnviroData.QualityAssurance;
using Hatfield.EnviroData.QualityAssurance.DataQualityCheckingData;
using Hatfield.EnviroData.QualityAssurance.DataQualityCheckingTool;

namespace Hatfield.EnviroData.QualityAssurance.Test.DataQualityCheckingTool
{
    [TestFixture]
    public class SampleMatrixTypeCheckingToolTest
    {
        [Test]
        public void IsDataQualityChekcingDataSupportTest()
        {
            var testTool = new SampleMatrixTypeCheckingTool();

            var successTestQualityCheckingData = new StringCompareCheckingData("test", false, "test");
            var mockQualityCheckingData = new Mock<IDataQualityCheckingData>();

            Assert.True(testTool.IsDataQualityChekcingDataSupport(successTestQualityCheckingData));
            Assert.False(testTool.IsDataQualityChekcingDataSupport(mockQualityCheckingData.Object));
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
