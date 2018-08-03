
using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using Microsoft.Data.Sqlite;
using Ninject;
using WikiGraph.Interfaces;
using WikiGraph.Interfaces.Services;

namespace WikiGraph.Core
{
    class DataCollector: IDataCollector
    {
        HtmlWeb web;
        public static SqliteConnection databaseConnection;

        [Inject]
        public IUrlStackService StackService { private get; set; }

        [Inject]
        public IEdgeService EdgeService { private get; set; }

        [Inject]
        public IWebService WebService { private get; set; }

        public INodeService NodeService { private get; set; }
        List<Tuple<string, string>> edges;

        public DataCollector()
        {
            web = new HtmlWeb();
        }

        public void Run()
        {
            Console.CancelKeyPress += new ConsoleCancelEventHandler(this.OnExit);

            using (databaseConnection = new SqliteConnection("" + new SqliteConnectionStringBuilder { DataSource = "hello.db" }))
            {
                databaseConnection.Open();

                var prog = new DataCollector();
                edges = new List<Tuple<string, string>>();

                //while (stack.Count().Result > 0)
                for(var i = 0; i < 10; i++)
                {
                    var url = StackService.GetUrlFromStack().Result;

                    var info = WebService.GetInfoFromUrl(url).Result;
                    if (info == null)
                    {
                        continue;
                    }

                    NodeService.InsertNewNode(url, info.Html);
                    foreach (var x in info.Urls)
                    {
                        StackService.PutUrlInStack("https://en.wikipedia.org" + x).Wait();
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
            Console.WriteLine("saved");
            StackService.CommitAllUrlsToDatabase().Wait();
            EdgeService.AddEdges(edges);
        }
    }
}