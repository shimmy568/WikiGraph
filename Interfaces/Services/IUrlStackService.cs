
using System.Threading.Tasks;

namespace WikiGraph.Interfaces.Services
{
    public interface IUrlStackService
    {
        Task<string> GetUrlFromStack();
        Task PutUrlInStack(string url);
        Task<int> Count();
        Task CommitAllUrlsToDatabase();
    }
}