
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WikiGraph.Core.Repositories
{
    public interface IEdgeRepository
    {
        Task AddEgde(string sourceUrl, string destUrl);
        Task AddEdges(IEnumerable<Tuple<string, string>> edges);
    }
}