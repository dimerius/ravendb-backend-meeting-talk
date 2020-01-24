using Raven.Client.Documents;

namespace AwesomeRaven.Raven
{
    public interface IRavenClient
    {
        IDocumentStore Store { get; }
    }
}
