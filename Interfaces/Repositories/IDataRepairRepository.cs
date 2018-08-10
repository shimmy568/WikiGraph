
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WikiGraph.Interfaces.Repositories{
    public interface IDataRepairRepository
    {
        Task RepairUrlStackFromEdges();
        IEnumerable<string> GetAllHtmlForNodes();
        IEnumerable<string> FilterUrlsThatAppearInDatabase(IEnumerable<string> urls);
    }
}