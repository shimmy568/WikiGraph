using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WikiGraph.Core.Models;

namespace WikiGraph.Interfaces.Repositories
{
    public interface INodeRepository
    {
        Task InsertNewNode(string url, string html, DateTime timeRetrieved, string title);
        Task<Node> GetNodeByID(int id);
        Task<Node> GetNodeByUrl(string url);
        IEnumerable<Node> GetNodesByUrls(IEnumerable<string> urls);
    }
}