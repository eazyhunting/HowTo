using HowTo.Services;
using HowTo.Services.Interfaces;
using HowTo.Shared.Version;

namespace HowTo.Tests
{
    public class VersionServiceTests
    {
        private readonly IVersionService _sut;

        public VersionServiceTests() {
            _sut = new VersionService();
        }

        [Theory]
        [InlineData("1.0.0", "2.0.0", VersionResultEnum.LessThan)]
        [InlineData("3.0.0", "2.0.0", VersionResultEnum.GreaterThan)]
        [InlineData("1.0.0", "1.0.0", VersionResultEnum.Equal)]
        public async void CompareTheory_Successful(string baseVersion, string versionToCompare, VersionResultEnum expectedResult)
        {
            // arrange
            var model = new VersionComparisonModel();
            model.BaseVersion = baseVersion;
            model.VersionToCompare = versionToCompare;

            // act
            var actual = await _sut.Compare(model);

            // assert
            Assert.True(actual.Result == expectedResult);
        }

        [Fact]
        public async void CompareThrowsException()
        {
            // arrange
            var model = new VersionComparisonModel();
           
            model.BaseVersion = "asdf";

            // act 
            Task act() => _sut.Compare(model);

            // assert
            await Assert.ThrowsAsync<ArgumentException>(act);
        }
    }
}