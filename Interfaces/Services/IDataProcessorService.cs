
using WikiGraph.Core.Enums;
using System.Collections.Generic;

namespace WikiGraph.Interfaces.Services
{
    public interface IDataProcessorService
    {
        void Run(string type, IEnumerable<string> args);
    }
}