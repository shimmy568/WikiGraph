
using System.Threading.Tasks;
using WikiGraph.Core.Models;

namespace WikiGraph.Interfaces.Services
{
    public interface INodeService
    {
        Task<Node> GetNodeByUrl(string url);
        Task<Node> GetNodeByID(int id);
        Task InsertNewNode(string url, string html);
    }
}