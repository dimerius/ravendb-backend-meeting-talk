using System.IO;
using Microsoft.Extensions.Configuration;

namespace AwesomeRaven.Tests
{
    public class AppTestConfiguration
    {
        private static IConfiguration _configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.test.local.json", false, reloadOnChange: true)
            .Build();

        public IConfiguration Configuration => _configuration;
    }
}
