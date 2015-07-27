using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;

using Hatfield.EnviroData.QualityAssurance.DataQualityCheckingRules;

namespace Hatfield.EnviroData.QualityAssurance.Test.DataQualityCheckingRules
{
    [TestFixture]
    public class StringCompareCheckingRuleTest
    {
        [Test]
        public void ConstructorTest()
        {
            var testRule = new StringCompareCheckingRule("expected value", true, "test correction value");

            Assert.NotNull(testRule);
            Assert.AreEqual("expected value", testRule.ExpectedValue);
            Assert.AreEqual("test correction value", testRule.CorrectionValue);
            Assert.True(testRule.IsCaseSensitive);
        }
    }
}
