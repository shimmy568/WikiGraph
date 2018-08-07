
using WikiGraph.Core.Enums;

namespace WikiGraph.Interfaces.Services
{
    public interface IDataProcessorService
    {
        void Run(string type);
    }
}