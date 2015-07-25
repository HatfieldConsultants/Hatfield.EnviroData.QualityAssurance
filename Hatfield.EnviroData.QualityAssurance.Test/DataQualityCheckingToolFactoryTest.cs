using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;

using Hatfield.EnviroData.QualityAssurance;

namespace Hatfield.EnviroData.QualityAssurance.Test
{
    [TestFixture]
    public class DataQualityCheckingToolFactoryTest
    {
        [Test]
        public void GenerateDataQualityChekcingToolTest()
        {
            var configurationToTest = new DataQualityCheckingToolConfiguration();

            var factory = new DataQualityCheckingToolFactory();

            Assert.Throws(typeof(NotImplementedException), () => factory.GenerateDataQualityCheckingTool(configurationToTest));


        }
    }
}
