using System;
using System.Linq;
using Ninject.Modules;
using WikiGraph.Core.Repositories;
using WikiGraph.Interfaces.Repositories;
using WikiGraph.Core.Services;
using WikiGraph.Interfaces.Services;
using WikiGraph.Core;
using WikiGraph.Interfaces;

namespace WikiGraph
{
    public class Bindings : NinjectModule
    {
        public override void Load()
        {
            Bind<IDataCollector>().To<DataCollector>();
            BindRepositories();
            BindServices();
        }

        private void BindRepositories()
        {
            Bind<INodeRepository>().To<NodeRepository>();
            Bind<IUrlStackRepository>().To<UrlStackRepository>();
            Bind<IEdgeRepository>().To<EdgeRepository>();
            Bind<IWebRepository>().To<WebRepository>();
            Bind<IDataProcessorRepository>().To<DataProcessorRepository>();
        }

        private void BindServices()
        {
            Bind<INodeService>().To<NodeService>();
            Bind<IUrlStackService>().To<UrlStackService>();
            Bind<IEdgeService>().To<EdgeService>();
            Bind<IWebService>().To<WebService>();
            Bind<IDataProcessorService>().To<DataProcessorService>();
            Bind<IDataRepairService>().To<DataRepairService>();
        }
    }
}