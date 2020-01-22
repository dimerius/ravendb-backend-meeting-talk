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

        public void Execute(string input) => _logger.LogInformation("It works! {0}");
    }
}
