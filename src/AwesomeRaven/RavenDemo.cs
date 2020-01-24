using System.Threading.Tasks;
using AwesomeRaven.Entities;
using AwesomeRaven.Raven;
using Microsoft.Extensions.Logging;

namespace AwesomeRaven
{
    public class RavenDemo
    {
        private IRavenClient _raven;
        private ILogger<RavenDemo> _logger;

        public RavenDemo(IRavenClient raven, ILogger<RavenDemo> logger) =>
            (_raven, _logger) = (raven, logger);

        public async Task Execute(string input)
        {
            using var session = _raven.Store.OpenAsyncSession();
            var employee = await session.LoadAsync<Employee>("employees/8-A");
            
            _logger.LogInformation("My favourite employee is {0}", employee.FirstName);
        }
    }
}
