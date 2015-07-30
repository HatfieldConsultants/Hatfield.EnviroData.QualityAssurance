using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Hatfield.EnviroData.Core;
using Hatfield.WQDefaultValueProvider;
using Hatfield.EnviroData.Core.Repositories;
using Hatfield.EnviroData.QualityAssurance;
using Hatfield.EnviroData.WQDataProfile;
using Hatfield.EnviroData.QualityAssurance.DataFetchCriterias;
using Hatfield.EnviroData.QualityAssurance.DataQualityCheckingRules;
using Hatfield.EnviroData.QualityAssurance.DataQualityCheckingTool;
using Hatfield.EnviroData.WQDataProfile.Repositories;

namespace Hatfield.EnviroData.QualityAssurance.Execute
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var dbContext = new ODM2Entities();

            var relationActionTypeRepository = new Repository<CV_RelationshipType>(dbContext);
            var wqDataRepository = new WQDataRepository(dbContext);

            var staticWQDefaultValueProvider = new StaticWQDefaultValueProvider();
            staticWQDefaultValueProvider.Init();

            Console.WriteLine("Start Quality Checking Process...");

            var qcChainConfiguration = ConfigureQualityCheckingChain(true, wqDataRepository, "Test", false, "Water");

            var versioningHelper = new DataVersioningHelper(staticWQDefaultValueProvider);
            var factory = new DataQualityCheckingToolFactory(versioningHelper, relationActionTypeRepository);
            var qualiltyChecker = new WaterQualityDataQualityChecker(qcChainConfiguration, factory, wqDataRepository);

            var qcResults = qualiltyChecker.Check();

            foreach(var result in qcResults)
            {
                Console.WriteLine(string.Format("{0}: {1}", result.Level, result.Message));
            
            }

            Console.WriteLine("Press any keys to continue...");
            Console.ReadLine();
        }

        private static DataQualityCheckingChainConfiguration ConfigureQualityCheckingChain(bool needCorrection,
                                                                                            IWQDataRepository wqDataRepository, 
                                                                                            string expectedString, 
                                                                                            bool isCaseSensitive, 
                                                                                            string correctionValue)
        {
            var chainConfiguration = new DataQualityCheckingChainConfiguration();

            chainConfiguration.NeedToCorrectData = needCorrection;
            chainConfiguration.DataFetchCriteria = new GetAllWaterQualitySampleDataCriteria(wqDataRepository);

            var sampleMatrixCheckingRuleConfiguration = new DataQualityCheckingToolConfiguration();
            sampleMatrixCheckingRuleConfiguration.DataQualityCheckingToolType = typeof(SampleMatrixTypeCheckingTool);
            sampleMatrixCheckingRuleConfiguration.DataQualityCheckingRule = new StringCompareCheckingRule(expectedString,
                                                                                                          isCaseSensitive,
                                                                                                          correctionValue
                                                                                                         );

            chainConfiguration.ToolsConfiguration = new List<DataQualityCheckingToolConfiguration> { sampleMatrixCheckingRuleConfiguration };

            return chainConfiguration;
        }
    }
}
