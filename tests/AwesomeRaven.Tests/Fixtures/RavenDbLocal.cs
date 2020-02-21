using System;
using AwesomeRaven.Raven;
using Microsoft.Extensions.Configuration;
using Raven.Client.Documents;

namespace AwesomeRaven.Tests.Fixtures
{
    public class RavenDbTestClient : IRavenClient
    {
        private static volatile IDocumentStore _store;
        public IDocumentStore Store => _store;

        public RavenDbTestClient()
        {
            var ravenConfiguration = new RavenConfiguration();
            AppTestConfiguration.Configuration.GetSection("RavenDb:AwesomeRaven").Bind(ravenConfiguration);
            
            _store = new RavenClient(ravenConfiguration).Store;
        }
    }
}
