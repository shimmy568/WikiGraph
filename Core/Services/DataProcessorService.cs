
using System;
using WikiGraph.Interfaces;
using System.Linq;
using System.Collections.Generic;
using WikiGraph.Core.Enums;
using WikiGraph.Interfaces.Services;
using Ninject;
using WikiGraph.Interfaces.Repositories;

namespace WikiGraph.Core.Services
{
    class DataProcessorService : IDataProcessorService
    {

        [Inject]
        public IDataProcessorRepository DataProcessorRepository { private get; set; }

        public void Run(string type)
        {
            DataProcessingType parsedType;
            if (Enum.TryParse(type, out parsedType))
            {
                if(parsedType == DataProcessingType.MostReferencedCsv){
                    StartMostReferencedCsv();
                }
            }
            else
            {
                Console.WriteLine("Please use one of the following types for data processing: ");
                foreach (var curType in Enum.GetNames(typeof(DataProcessingType)))
                {
                    if (curType == DataProcessingType.NoneSet.ToString())
                    {
                        continue;
                    }

                    Console.WriteLine(curType);
                }
            }
        }

        private void StartMostReferencedCsv(){
            var mostReferencedList = DataProcessorRepository
        }
    }
}