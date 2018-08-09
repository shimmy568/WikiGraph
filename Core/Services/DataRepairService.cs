
using HtmlAgilityPack;
using Ninject;
using WikiGraph.Interfaces.Repositories;
using WikiGraph.Interfaces.Services;

namespace WikiGraph.Core.Services
{
    public class DataRepairService : IDataRepairService
    {

        [Inject]
        public IDataRepairRepository DataRepairRepository { private get; set; }

        [Inject]
        public IWebService WebService { private get; set; }

        [Inject]
        public INodeService NodeService { private get; set; }

        public void Run()
        {
            DataRepairRepository.RepairUrlStackFromEdges().Wait();

            var allHtml = DataRepairRepository.GetAllHtmlForNodes();

            foreach (var html in allHtml)
            {
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(html);

                var links = WebService.GetLinksFromHtmlDocument(htmlDoc);

                var 
            }
        }

    }
}