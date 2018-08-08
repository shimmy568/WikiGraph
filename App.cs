using System;
using HtmlAgilityPack;
using System.Linq;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using Ninject;
using WikiGraph.Core.Repositories;
using WikiGraph.Interfaces.Repositories;
using WikiGraph.Core.Services;
using WikiGraph.Interfaces.Services;
using WikiGraph.Interfaces;

namespace WikiGraph
{
    class App
    {
        public static SqliteConnection databaseConnection;
        public static void Main(string[] args)
        {
            var choice = 0;
            if (args.Length == 0 || !Int32.TryParse(args[0], out choice) || choice <= 0 || choice >= 4)
            {
                Console.WriteLine("Please enter in a command line arg for one of the following choices");
                Console.WriteLine("1: Collect data");
                Console.WriteLine("2: Graph the data");
                Console.WriteLine("3: Repair data");
                Console.WriteLine("Just use one of those two numbers");
                return;
            }

            if (choice == 1)
            {
                // Starting the main data collection process
                var kern = new KernelConfiguration(new Bindings()).BuildReadonlyKernel();
                var mainThing = kern.Get<IDataCollector>();
                mainThing.Run();
            }
            else if (choice == 2)
            {
                var kern = new KernelConfiguration(new Bindings()).BuildReadonlyKernel();
                var mainThing = kern.Get<IDataProcessorService>();
                var type = "";
                if (args.Length > 1)
                {
                    type = args[1];
                }
                mainThing.Run(type);
            }
            else if (choice == 3)
            {
                var kern = new KernelConfiguration(new Bindings()).BuildReadonlyKernel();
                var mainThing = kern.Get<IDataRepairService>();
                mainThing.Run();
            }
        }
    }
}