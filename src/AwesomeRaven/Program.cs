using System;
using System.IO;
using System.Threading.Tasks;
using AwesomeRaven.Raven;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace AwesomeRaven
{
    class Program
    {
        private static IConfiguration AppConfiguration { get; } = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", false, reloadOnChange: true)
            .Build();

        public static async Task Main(string[] args)
        {
            //setup our DI
            var serviceProvider = new ServiceCollection()
                .AddLogging(configure =>
                {
                    configure.AddConfiguration(AppConfiguration.GetSection("Logging"));
                    configure.AddConsole();
                })
                .AddTransient<RavenDemo>()
                .AddSingleton<IRavenClient, RavenClient>()
                .AddStronglyTypedConfiguration<RavenConfiguration>(AppConfiguration.GetSection("RavenDb:AwesomeRaven"))
                .BuildServiceProvider();


            var logger = serviceProvider.GetService<ILoggerFactory>()
                .CreateLogger<Program>();

            logger.LogInformation("Finished configuring logging and dependency injection!");
            logger.LogInformation("Application is ready!");

            var demo = serviceProvider.GetService<RavenDemo>();

            var result = await demo.Execute(PerformOperation.SuggestEmployeeNames);

            logger.LogInformation(JsonConvert.SerializeObject(result, Formatting.Indented));
        }
    }
}
