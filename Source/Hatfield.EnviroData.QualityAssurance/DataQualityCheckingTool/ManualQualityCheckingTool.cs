using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Hatfield.EnviroData.Core;
using Hatfield.EnviroData.DataAcquisition.ESDAT.Converters;
using Hatfield.EnviroData.QualityAssurance.DataQualityCheckingRules;
using Hatfield.EnviroData.WQDataProfile;

namespace Hatfield.EnviroData.QualityAssurance.DataQualityCheckingTool
{
    public class ManualQualityCheckingTool : IDataQualityCheckingTool
    {
        private IDataVersioningHelper _dataVersioningHelper;
        private IRepository<CV_RelationshipType> _relationShipTypeCVRepository;

        /// <summary>
        /// Correct the measurement value
        /// </summary>
        /// <param name="versioningHelper"></param>
        /// <param name="relationShipTypeCVRepository"></param>
        public ManualQualityCheckingTool(IDataVersioningHelper versioningHelper, IRepository<CV_RelationshipType> relationShipTypeCVRepository)
        {
            _dataVersioningHelper = versioningHelper;
            _relationShipTypeCVRepository = relationShipTypeCVRepository;
        }

        public IQualityCheckingResult Check(object data, IDataQualityCheckingRule dataQualityCheckingRule)
        {
            throw new NotImplementedException();
        }

        public IQualityCheckingResult Correct(object data, IDataQualityCheckingRule dataQualityCheckingRule)
        {
            throw new NotImplementedException();
        }

        public bool IsDataQualityChekcingRuleSupported(IDataQualityCheckingRule dataQualityCheckingRule)
        {
            throw new NotImplementedException();
        }

        public bool IsDataSupport(object data)
        {
            throw new NotImplementedException();
        }
    }
}
