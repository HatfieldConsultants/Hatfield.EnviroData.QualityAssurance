using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;
using Moq;

using Hatfield.EnviroData.Core;
using Hatfield.EnviroData.WQDataProfile;
using Hatfield.EnviroData.QualityAssurance.DataFetchCriterias;

namespace Hatfield.EnviroData.QualityAssurance.Test.DataFetchCriterias
{
    [TestFixture]
    public class GetAllWaterQualitySampleDataCriteriaTest
    {
        [Test]
        public void NullArgumentConstructorTest()
        {
            Assert.Throws(typeof(ArgumentNullException), () => new GetAllWaterQualitySampleDataCriteria(null));
        }

        [Test]
        public void SuccessConstructorTest()
        {
            var testSampleAction = new Hatfield.EnviroData.Core.Action();

            var mockRepository = new Mock<IWQDataRepository>();
            mockRepository.Setup(x => x.GetAllWQSampleDataActions())
                            .Returns(new List<Hatfield.EnviroData.Core.Action> { 
                                testSampleAction
                            });
            var testCriteria = new GetAllWaterQualitySampleDataCriteria(mockRepository.Object);

            Assert.NotNull(testCriteria);
            Assert.AreEqual("GetAllWaterQualityDataCriteria: fetch all water quality data in the database", testCriteria.CriteriaDescription);

            var fetchedData = testCriteria.FetchData().Cast<Hatfield.EnviroData.Core.Action>();

            Assert.NotNull(fetchedData);
            Assert.AreEqual(1, fetchedData.Count());
            Assert.AreEqual(testSampleAction, fetchedData.ElementAt(0));
            
        }
    }
}
