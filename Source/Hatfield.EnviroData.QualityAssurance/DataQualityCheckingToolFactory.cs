using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Hatfield.EnviroData.Core;
using Hatfield.EnviroData.WQDataProfile;
using Hatfield.EnviroData.QualityAssurance.DataQualityCheckingTool;

namespace Hatfield.EnviroData.QualityAssurance
{
    public class DataQualityCheckingToolFactory : IDataQualityCheckingToolFactory
    {
        private IDataVersioningHelper _versionHelper;
        private IRepository<CV_RelationshipType> _relationShipTypeCVRepository;

        public DataQualityCheckingToolFactory(IDataVersioningHelper versionHelper, IRepository<CV_RelationshipType> relationShipTypeCVRepository)
        {
            _versionHelper = versionHelper;
            _relationShipTypeCVRepository = relationShipTypeCVRepository;
        }

        public IDataQualityCheckingTool GenerateDataQualityCheckingTool(DataQualityCheckingToolConfiguration toolConfiguration)
        {
            if (toolConfiguration.DataQualityCheckingToolType == typeof(SampleMatrixTypeCheckingTool))
            {
                return new SampleMatrixTypeCheckingTool(_versionHelper, _relationShipTypeCVRepository);
            }
            else
            {
                throw new ArgumentException("Data Quality Checking Tool factory could not generate tool for " + toolConfiguration.DataQualityCheckingToolType.Name);
            }
            
        }
    }
}
