using System.Threading.Tasks;

namespace AwesomeRaven.Tests.Fixtures
{
    public interface IHaveDataSetup
    {
        Task PrepareData();
    }
}
