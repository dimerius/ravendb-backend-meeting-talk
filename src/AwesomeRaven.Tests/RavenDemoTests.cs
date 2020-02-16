using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AwesomeRaven.Raven;
using AwesomeRaven.Tests.Fixtures;
using AwesomeRaven.Tests.Fixtures.Employees.SearchInEmployees;
using FakeItEasy;
using Microsoft.Extensions.Logging;
using Shouldly;
using Xunit;

namespace AwesomeRaven.Tests
{
    public class RavenDemoTests : IClassFixture<SearchByFullNameFragmentsFixture>
    {
        private readonly SearchByFullNameFragmentsFixture _raven;

        private readonly ILogger<RavenDemo> _fakeLogger = A.Fake<ILogger<RavenDemo>>();

        private RavenDemo RavenDemo => new RavenDemo(_raven, _fakeLogger);

        public RavenDemoTests(SearchByFullNameFragmentsFixture raven) => _raven = raven;


        [Theory]
        [InlineData("Laura Callaha", "Laura Callahan")]
        [InlineData("Robert Ki", "Robert King")]
        [InlineData("Rob King", "Robert King")]
        public async Task Should_Suggest_Employee_Fullname(string input, string expected)
        {
            await _raven.PrepareData();
            
            var result = await RavenDemo.SuggestEmployeeNamesAsync(input);
            
            result.ShouldContain(expected);
        }
    }
}
