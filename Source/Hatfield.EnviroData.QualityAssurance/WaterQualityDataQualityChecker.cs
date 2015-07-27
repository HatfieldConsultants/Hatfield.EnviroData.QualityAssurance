using System.Collections.Generic;
using System.Linq;

using Hatfield.EnviroData.WQDataProfile;

namespace Hatfield.EnviroData.QualityAssurance
{
    public class WaterQualityDataQualityChecker
    {
        private DataQualityCheckingChainConfiguration _chainConfiguration;
        private IDataQualityCheckingToolFactory _dataQualityCheckingToolFactory;
        private IWQDataRepository _wqDataRepository;

        public WaterQualityDataQualityChecker(
                                                DataQualityCheckingChainConfiguration chainConfiguration,
                                                IDataQualityCheckingToolFactory dataQualityCheckingToolFactory,
                                                IWQDataRepository wqDataRepository
                                             )
        {
            _chainConfiguration = chainConfiguration;
            _dataQualityCheckingToolFactory = dataQualityCheckingToolFactory;
            _wqDataRepository = wqDataRepository;
        }

        public IEnumerable<IQualityCheckingResult> Check()
        {
            var qualityCheckingResult = new List<IQualityCheckingResult>();

            var allDataToCheck = _chainConfiguration.DataFetchCriteria.FetchData();
            qualityCheckingResult.Add(new QualityCheckingResult("Fetch data for quality chekcing by " + _chainConfiguration.DataFetchCriteria.CriteriaDescription, false, QualityCheckingResultLevel.Info));

            if (allDataToCheck == null || !allDataToCheck.Any())
            {
                //no data found, done
                qualityCheckingResult.Add(new QualityCheckingResult("No data found by the criteria, quality checking finishes.", false, QualityCheckingResultLevel.Info));
            }
            else
            {
                if (_chainConfiguration.ToolsConfiguration != null)
                {
                    //data found, QA data
                    var needToUpdateData = false;

                    foreach (var toolConfiguration in _chainConfiguration.ToolsConfiguration)
                    {
                        var dataQualityCheckingTool = _dataQualityCheckingToolFactory.GenerateDataQualityCheckingTool(toolConfiguration);

                        foreach (var dataToCheck in allDataToCheck)
                        {
                            var qcResult = dataQualityCheckingTool.Check(dataToCheck, toolConfiguration.DataQualityCheckingRule);
                            qualityCheckingResult.Add(qcResult);
                            //if need correction
                            if (qcResult.NeedCorrection && _chainConfiguration.NeedToCorrectData)
                            {
                                dataQualityCheckingTool.Correct(dataToCheck, toolConfiguration.DataQualityCheckingRule);
                                needToUpdateData = true;
                            }
                        }
                    }

                    if (needToUpdateData)
                    {
                        //save corrected data to database
                        _wqDataRepository.SaveChanges();
                        var updateResult = new QualityCheckingResult("Data correction updated.", false, QualityCheckingResultLevel.Info);
                        qualityCheckingResult.Add(updateResult);
                    }
                }
                
            }

            return qualityCheckingResult;
        }
    }
}