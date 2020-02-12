using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AwesomeRaven
{
    public class RavenConfiguration
    {
        public string[]? Urls { get; set; }
        public string? DatabaseGroupName { get; set; }
    }

    public static class AwesomeRavenConfigurationExtensions
    {
        public static IServiceCollection AddStronglyTypedConfiguration<T>(this IServiceCollection services,
            IConfiguration configuration) where T : class, new()
        {
            var stronglyTypedConfiguration = new T();
            configuration.Bind(stronglyTypedConfiguration);
            services.AddSingleton(stronglyTypedConfiguration);
            return services;
        }
    }
}
