
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WikiGraph.Interfaces.Services
{
    interface IEdgeService
    {
        Task AddEdge(string sourceUrl, string destUrl);
        Task AddEdges(IEnumerable<Tuple<string, string>> relations);
    }
}