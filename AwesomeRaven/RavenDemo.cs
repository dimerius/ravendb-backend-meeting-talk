using Microsoft.Extensions.Logging;

namespace AwesomeRaven
{
    public class RavenDemo
    {
        private RavenConfiguration _ravenConfiguration;
        private ILogger<RavenDemo> _logger;

        public RavenDemo(RavenConfiguration ravenConfiguration, ILogger<RavenDemo> logger) =>
            (_ravenConfiguration, _logger) = (ravenConfiguration, logger);

        public void Execute(string input) => _logger.LogInformation("It works! {0}", _ravenConfiguration.DatabaseGroupName );
    }
}
