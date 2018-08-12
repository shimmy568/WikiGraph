
using System;
using System.Linq;
using System.Collections.Generic;
using HtmlAgilityPack;
using Ninject;
using WikiGraph.Interfaces.Repositories;
using WikiGraph.Interfaces.Services;
using Microsoft.Data.Sqlite;

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

        [Inject]
        public IUrlStackRepository UrlStackRepository { private get; set; }
        public void Run()
        {
            using (App.databaseConnection = new SqliteConnection("" + new SqliteConnectionStringBuilder { DataSource = "hello.db" }))
            {
                App.databaseConnection.Open();

                DataRepairRepository.RepairUrlStackFromEdges().Wait();

                var allHtml = DataRepairRepository.GetAllHtmlForNodes().ToList();

                foreach (var html in allHtml)
                {
                    var htmlDoc = new HtmlDocument();
                    htmlDoc.LoadHtml(html);

                    var links = WebService.GetLinksFromHtmlDocument(htmlDoc);

                    var newLinks = GetLinksThatDontAppearInDB(links);

                    UrlStackRepository.AddUrlsToStack(newLinks);
                }
            }
        }

        private IEnumerable<string> GetLinksThatDontAppearInDB(IEnumerable<string> links)
        {
            var urlsThatAppear = DataRepairRepository.FilterUrlsThatAppearInDatabase(links).ToDictionary(x => x);
            return links.Where(x => !urlsThatAppear.ContainsKey(x));
        }
    }
}