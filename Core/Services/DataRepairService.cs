
using Ninject;
using WikiGraph.Interfaces.Repositories;
using WikiGraph.Interfaces.Services;

namespace WikiGraph.Core.Services
{
    public class DataRepairService: IDataRepairService
    {

        [Inject]
        public IDataRepairRepository DataRepairRepository { private get; set; }

        public void Run(){
            DataRepairRepository.RepairUrlStackFromEdges();
        }

    }
}