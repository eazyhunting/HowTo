using HowTo.Services.Domain;
using HowTo.Services.Interfaces;
using HowTo.Shared.Version;

namespace HowTo.Services
{
    public class VersionService : IVersionService
    {
        public async Task<VersionComparisonResultModel> CompareAsync(VersionComparisonModel comparisonModel)
        {
            var version1 = new FivePartVersion(comparisonModel.BaseVersion);
            var version2 = new FivePartVersion(comparisonModel.VersionToCompare);

            Enum.TryParse<VersionResultEnum>(
                version1.CompareTo(version2).ToString(), out var enumValue);

            return await Task.Run(() =>
            {
                return Task.FromResult(new VersionComparisonResultModel(comparisonModel)
                {
                    Result = enumValue
                });
            });
        }
    }
}
