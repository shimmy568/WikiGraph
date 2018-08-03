
using System;
using System.Threading.Tasks;
using Ninject;
using WikiGraph.Core.Models;
using WikiGraph.Interfaces.Repositories;
using WikiGraph.Interfaces.Services;

namespace WikiGraph.Core.Services
{


    public class NodeService : INodeService
    {
        [Inject]
        public INodeRepository NodeRepository { private get; set; }

        public async Task<Node> GetNodeByID(int id)
        {
            return await NodeRepository.GetNodeByID(id);
        }

        public async Task<Node> GetNodeByUrl(string url)
        {
            return await NodeRepository.GetNodeByUrl(url);
        }

        public async Task InsertNewNode(string url, string html)
        {
            await NodeRepository.InsertNewNode(url, html, DateTime.Now);
        }
    }
}