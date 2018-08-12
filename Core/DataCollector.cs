
using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using Microsoft.Data.Sqlite;
using Ninject;
using WikiGraph.Core.Models;
using WikiGraph.Interfaces;
using WikiGraph.Interfaces.Services;

namespace WikiGraph.Core
{
    class DataCollector: IDataCollector
    {
        HtmlWeb web;

        [Inject]
        public IUrlStackService StackService { private get; set; }

        [Inject]
        public IEdgeService EdgeService { private get; set; }

        [Inject]
        public IWebService WebService { private get; set; }

        [Inject]
        public INodeService NodeService { private get; set; }
        List<Tuple<string, string>> edges;

        public DataCollector()
        {
            web = new HtmlWeb();
        }

        public void Run()
        {
            Console.CancelKeyPress += new ConsoleCancelEventHandler(this.OnExit);

            using (App.databaseConnection = new SqliteConnection("" + new SqliteConnectionStringBuilder { DataSource = "hello.db" }))
            {
                App.databaseConnection.Open();

                var prog = new DataCollector();
                edges = new List<Tuple<string, string>>();

                while (StackService.Count().Result > 0)
                {
                    var url = StackService.GetUrlFromStack().Result;

                    var info = WebService.GetInfoFromUrl(url).Result;
                    if (info == null)
                    {
                        continue;
                    }

                    NodeService.InsertNewNode(url, info.Html, info.Title);
                    foreach (var x in info.Urls)
                    {
                        StackService.PutUrlInStack(x).Wait();
                        edges.Add(new Tuple<string, string>(url, x));
                    }

                    if (edges.Count > 10000)
                    {
                        EdgeService.AddEdges(edges).Wait();
                        edges = new List<Tuple<string, string>>();
                    }

                    Console.WriteLine("Got " + info.Urls.Count() + " new urls");
                }
                EdgeService.AddEdges(edges).Wait();
                Console.WriteLine(StackService.Count().Result);
                StackService.CommitAllUrlsToDatabase().Wait();
                Console.WriteLine(StackService.Count().Result);
            }
        }

        public void OnExit(object sender, EventArgs e)
        {
            Console.WriteLine("saved all the good shit");
            StackService.CommitAllUrlsToDatabase().Wait();
            EdgeService.AddEdges(edges);
        }
    }
}