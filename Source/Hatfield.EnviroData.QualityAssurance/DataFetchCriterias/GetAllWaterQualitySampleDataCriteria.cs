using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Hatfield.EnviroData.WQDataProfile;

namespace Hatfield.EnviroData.QualityAssurance.DataFetchCriterias
{
    public class GetAllWaterQualitySampleDataCriteria : IDataFetchCriteria
    {
        private IWQDataRepository _wqDataRepository;

        public GetAllWaterQualitySampleDataCriteria(IWQDataRepository wqDataRepository)
        {
            if (wqDataRepository == null)
            {
                throw new ArgumentNullException("Water quality data repository is null. GetAllWaterQualityDataCriteria initial fail.");
            }
            else
            {
                _wqDataRepository = wqDataRepository;
            }
        }

        public IEnumerable<object> FetchData()
        {
            return _wqDataRepository.GetAllWQSampleDataActions();
        }

        public string CriteriaDescription
        {
            get
            {
                return "GetAllWaterQualityDataCriteria: fetch all water quality data in the database";
            }
        }
    }
}
