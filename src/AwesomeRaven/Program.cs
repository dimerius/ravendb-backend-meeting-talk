using System;
using System.IO;
using AwesomeRaven.Raven;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


namespace AwesomeRaven
{
    class Program
    {
        private static IConfiguration AppConfiguration => new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", false, reloadOnChange: true)
            .Build();
        
        static void Main(string[] args)
        {
            //setup our DI
            var serviceProvider = new ServiceCollection()
                .AddLogging(configure =>
                {
                    configure.AddConfiguration(AppConfiguration.GetSection("Logging"));
                    configure.AddConsole();
                })
                .AddTransient<RavenDemo>()
                .AddTransient<IRavenClient, RavenClient>()
                .AddStronglyTypedConfiguration<RavenConfiguration>(AppConfiguration.GetSection("RavenDb:AwesomeRaven"))
                .BuildServiceProvider();
            
            
            var logger = serviceProvider.GetService<ILoggerFactory>()
                .CreateLogger<Program>();
            
            logger.LogInformation("Finished configuring logging and dependency injection!");
            logger.LogInformation("Application is ready!");

            var demo = serviceProvider.GetService<RavenDemo>();
            
            var input = Console.ReadLine();
            demo.Execute(input).Wait();
        }
    }
}
