using HowTo.Shared.Version;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HowTo.Services.Interfaces
{
    public interface IVersionService
    {
        Task<VersionComparisonResultModel> CompareAsync(VersionComparisonModel comparisonModel);
    }
}
