
using System.Collections.Generic;
using WikiGraph.Core.Models;

namespace WikiGraph.Interfaces.Repositories
{
    public interface IDataProcessorRepository
    {
        IEnumerable<UrlCountPair> GetReferenceCountsForAllNodes();
    }
}