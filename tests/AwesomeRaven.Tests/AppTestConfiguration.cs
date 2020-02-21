using System.IO;
using Microsoft.Extensions.Configuration;

namespace AwesomeRaven.Tests
{
    public static class AppTestConfiguration
    {
        public static IConfiguration Configuration { get; } = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.test.local.json", false, reloadOnChange: true)
            .Build();
    }
}
