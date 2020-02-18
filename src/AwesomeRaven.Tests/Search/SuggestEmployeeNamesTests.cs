using System.Threading.Tasks;
using AwesomeRaven.Raven;
using AwesomeRaven.Tests.Fixtures.Employees.SearchInEmployees;
using FakeItEasy;
using Microsoft.Extensions.Logging;
using Shouldly;
using Xunit;

namespace AwesomeRaven.Tests.Search
{
    [Trait("Category", "Employee")]
    [Collection("Tests with RavenDb data")]
    public class SuggestEmployeeNamesTests : IClassFixture<SearchByFullNameFragmentsFixture>
    {
        private RavenDemo Sut => new RavenDemo(_raven, A.Fake<ILogger<RavenDemo>>());

        private readonly IRavenClient _raven;
        public SuggestEmployeeNamesTests(SearchByFullNameFragmentsFixture raven) => _raven = raven;

        [Theory]
        [InlineData("laura callahan", "Laura Callahan")]
        [InlineData("Laura Callaha", "Laura Callahan")]
        [InlineData("Robert Ki", "Robert King")]
        [InlineData("Rob King", "Robert King")]
        [InlineData("Tobert King", "Robert King")]
        [InlineData("Robert K", "Robert King")]
        [InlineData("L Callahan", "Laura Callahan")]
        public async Task ShouldSuggestEmployeeFullName(string input, string expected)
        {
            var result = await Sut.SuggestEmployeeNamesAsync(input);

            result.ShouldContain(expected);
        }

        [Theory]
        [InlineData("L C", "Laura Callahan")]
        [InlineData("Laura", "Laura Callahan")]
        [InlineData("Martin Watkins", "Laura Callahan")]
        [InlineData("", "Laura Callahan")]
        [InlineData("", "Robert King")]
        [InlineData("       ", "Robert King")]
        [InlineData("rob ki", "Robert King")]
        public async Task ShouldNotSuggestEmployeeFullName(string input, string expected)
        {
            var result = await Sut.SuggestEmployeeNamesAsync(input);

            result.ShouldNotContain(expected);
        }
    }
}
