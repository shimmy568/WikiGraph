
using System;
using WikiGraph.Interfaces;
using System.Linq;
using System.Collections.Generic;
using WikiGraph.Core.Enums;

namespace WikiGraph.Core
{
    class DataProcesser : IDataProcesser
    {

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
            
        }
    }
}