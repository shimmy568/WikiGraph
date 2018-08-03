
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WikiGraph.Interfaces.Repositories
{
    public interface IUrlStackRepository
    {
        Task<List<string>> GetUrlsFromStack();
        Task AddUrlToStack(string url);
        Task<int> CountRowsInStack();
        Task<bool> IsUrlInDatabaseStack(string url);
        Task AddUrlsToStack(IEnumerable<string> urls);
    }
}