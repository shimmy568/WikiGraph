
using System.Threading.Tasks;
using Ninject;
using WikiGraph.Core.Repositories;
using WikiGraph.Interfaces.Services;
using System.Collections.Generic;
using System;
using System.Linq;

namespace WikiGraph.Core.Services
{
    public class EdgeService : IEdgeService
    {

        [Inject]
        public IEdgeRepository EdgeRepository { private get; set; }

        public async Task AddEdge(string sourceUrl, string destUrl)
        {
            await EdgeRepository.AddEgde(sourceUrl, destUrl);
        }

        public async Task AddEdges(IEnumerable<Tuple<string, string>> relations)
        {
            await EdgeRepository.AddEdges(relations);
        }
    }
}