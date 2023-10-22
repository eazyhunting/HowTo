using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HowTo.Shared.Version
{
    public class VersionComparisonResultModel
    {
        public VersionComparisonResultModel(VersionComparisonModel comparisonModel) { 
            VersionComparisonModel = comparisonModel;
        }

        public VersionResultEnum Result { get; set; } = 0;
        public VersionComparisonModel VersionComparisonModel { get; set; }
        public string Message => $"BaseVersion ({VersionComparisonModel.BaseVersion}) is {Enum.GetName(typeof(VersionResultEnum), Result)} VersionToCompare ({VersionComparisonModel.VersionToCompare})";
    }
}
