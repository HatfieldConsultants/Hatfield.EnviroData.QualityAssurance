using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;
using Moq;

using Hatfield.EnviroData.Core;
using Hatfield.EnviroData.QualityAssurance;
using Hatfield.EnviroData.QualityAssurance.DataQualityCheckingTool;
using Hatfield.EnviroData.WQDataProfile;

namespace Hatfield.EnviroData.QualityAssurance.Test
{
    [TestFixture]
    public class DataQualityCheckingToolFactoryTest
    {
        [Test]
        public void GenerateSampleMatrixTypeCheckingToolTest()
        {
            var mockVersionHelper = new Mock<IDataVersioningHelper>();
            var mockRepository = new Mock<IRepository<CV_RelationshipType>>();

            
            var configurationToTest = new DataQualityCheckingToolConfiguration();
            configurationToTest.DataQualityCheckingToolType = typeof(SampleMatrixTypeCheckingTool);

            var factory = new DataQualityCheckingToolFactory(mockVersionHelper.Object, mockRepository.Object);

            var generatedTool = factory.GenerateDataQualityCheckingTool(configurationToTest);

            Assert.NotNull(generatedTool);
            Assert.AreEqual(typeof(SampleMatrixTypeCheckingTool), generatedTool.GetType());
        }

        [Test]
        public void GenerateDataQualityChekcingToolFailTest()
        {
            var configurationToTest = new DataQualityCheckingToolConfiguration();
            configurationToTest.DataQualityCheckingToolType = typeof(string);

            var mockVersionHelper = new Mock<IDataVersioningHelper>();
            var mockRepository = new Mock<IRepository<CV_RelationshipType>>();
            var factory = new DataQualityCheckingToolFactory(mockVersionHelper.Object, mockRepository.Object);

            Assert.Throws(typeof(NotImplementedException), () => factory.GenerateDataQualityCheckingTool(configurationToTest));


        }
    }
}
