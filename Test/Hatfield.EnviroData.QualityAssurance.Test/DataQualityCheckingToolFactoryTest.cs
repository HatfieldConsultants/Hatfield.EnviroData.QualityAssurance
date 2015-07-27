using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;

using Hatfield.EnviroData.QualityAssurance;
using Hatfield.EnviroData.QualityAssurance.DataQualityCheckingTool;

namespace Hatfield.EnviroData.QualityAssurance.Test
{
    [TestFixture]
    public class DataQualityCheckingToolFactoryTest
    {
        [Test]
        public void GenerateSampleMatrixTypeCheckingToolTest()
        {
            var configurationToTest = new DataQualityCheckingToolConfiguration();
            configurationToTest.DataQualityCheckingToolType = typeof(SampleMatrixTypeCheckingTool);

            var factory = new DataQualityCheckingToolFactory();

            var generatedTool = factory.GenerateDataQualityCheckingTool(configurationToTest);

            Assert.NotNull(generatedTool);
            Assert.AreEqual(typeof(SampleMatrixTypeCheckingTool), generatedTool.GetType());
        }

        [Test]
        public void GenerateDataQualityChekcingToolFailTest()
        {
            var configurationToTest = new DataQualityCheckingToolConfiguration();
            configurationToTest.DataQualityCheckingToolType = typeof(string);

            var factory = new DataQualityCheckingToolFactory();

            Assert.Throws(typeof(NotImplementedException), () => factory.GenerateDataQualityCheckingTool(configurationToTest));


        }
    }
}
