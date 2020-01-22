using System;
using Raven.Client.Documents;

namespace AwesomeRaven.Raven
{
    public class RavenClient : IRavenClient
    {
        private readonly RavenConfiguration _configuration;


        private readonly Lazy<IDocumentStore> _store;

        public IDocumentStore Store => _store.Value;

        public RavenClient(RavenConfiguration configuration)
        {
            _configuration = configuration;
            _store = new Lazy<IDocumentStore>(CreateStore);
        }

        private IDocumentStore CreateStore() => new DocumentStore()
        {
            // Define the cluster node URLs (required)
            Urls = _configuration?.Urls,
            Database = _configuration?.DatabaseGroupName,
            //Certificate = new X509Certificate2("C:\\path_to_your_pfx_file\\cert.pfx")
        }.Initialize();
    }
}
