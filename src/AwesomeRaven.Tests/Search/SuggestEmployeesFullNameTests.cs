using System.Threading.Tasks;
using AwesomeRaven.Tests.Fixtures.Employees.SearchInEmployees;
using FakeItEasy;
using Microsoft.Extensions.Logging;
using Shouldly;
using Xunit;

namespace AwesomeRaven.Tests.Search
{
    [Trait("Category", "Employee")]
    [Collection("Tests over Employee collection")]
    public class SuggestEmployeesFullNameTests : IClassFixture<SearchByFullNameFragmentsFixture>
    {
        private readonly RavenDemo _sut;
        public SuggestEmployeesFullNameTests(SearchByFullNameFragmentsFixture raven)
        {
            _sut = new RavenDemo(raven, A.Fake<ILogger<RavenDemo>>());
        }
        
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
            var result = await _sut.SuggestEmployeeNamesAsync(input);
   
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
            var result = await _sut.SuggestEmployeeNamesAsync(input);
            
            result.ShouldNotContain(expected);
        }
    }
}
