﻿using Hatfield.EnviroData.WQDataProfile;
using System.Collections.Generic;
using System.Linq;

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
            qualityCheckingResult.Add(new QualityCheckingResult("Fetch data for quality chekcing by " + _chainConfiguration.DataFetchCriteria.CriteriaDescription, false));

            if (allDataToCheck == null && !allDataToCheck.Any())
            {
                //no data found, done
                qualityCheckingResult.Add(new QualityCheckingResult("No data found by the criteria, quality checking finishes.", false));
            }
            else
            {
                //data found, QA data
                var needToUpdateData = false;

                foreach (var toolConfiguration in _chainConfiguration.ToolsConfiguration)
                {
                    var dataQualityCheckingTool = _dataQualityCheckingToolFactory.GenerateDataQualityCheckingTool(toolConfiguration);

                    foreach (var dataToCheck in allDataToCheck)
                    {
                        var qcResult = dataQualityCheckingTool.Check(dataToCheck);
                        qualityCheckingResult.Add(qcResult);
                        //if need correction
                        if (qcResult.NeedCorrection && _chainConfiguration.NeedToCorrectData)
                        {
                            dataQualityCheckingTool.Correct(dataToCheck);
                            needToUpdateData = true;
                        }
                    }
                }

                if (needToUpdateData)
                {
                    //save corrected data to database
                    _wqDataRepository.SaveChanges();
                }
            }

            return qualityCheckingResult;
        }
    }
}