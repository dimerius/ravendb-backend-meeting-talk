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
    public class SearchForEmployeeByFullNameTests : IClassFixture<SearchByFullNameFragmentsFixture>
    {
        private RavenDemo Sut => new RavenDemo(_raven, A.Fake<ILogger<RavenDemo>>());

        private readonly IRavenClient _raven;
        
        public SearchForEmployeeByFullNameTests(SearchByFullNameFragmentsFixture raven) => _raven = raven;

        [Theory]
        [InlineData("laura cal", "Laura Callahan")]
        [InlineData("la call", "Laura Callahan")]
        public async Task ShouldReturnEmployeeFullName(string input, string expected)
        {
            var result = await Sut.SearchForEmployeeByFullNameAsync(input);

            result.ShouldContain(expected);
        }

        [Theory]
        [InlineData("lauc cal", "Laura Callahan")]
        [InlineData("sam call", "Laura Callahan")]
        public async Task ShouldNotReturnEmployeeFullName(string input, string expected)
        {
            var result = await Sut.SearchForEmployeeByFullNameAsync(input);

            result.ShouldNotContain(expected);
        }
    }
}
