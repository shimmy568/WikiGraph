using System;
using System.Collections.Generic;
using System.Linq;
using WikiGraph.Core.Models;
using WikiGraph.Interfaces.Repositories;

namespace WikiGraph.Core.Repositories
{
    class DataProcessorRepository: IDataProcessorRepository
    {

        public IEnumerable<UrlCountPair> GetReferenceCountsForAllNodes(){
            using (var trans = DataCollector.databaseConnection.BeginTransaction())
            {
                var selectCommand = DataCollector.databaseConnection.CreateCommand();
                selectCommand.Transaction = trans;
                selectCommand.CommandText = "SELECT Dest, COUNT(*) total FROM Edges GROUP BY Dest ORDER BY total DESC;";

                var urls = new List<string>();
                using (var reader = selectCommand.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        yield return new UrlCountPair(reader.GetString(0), reader.GetInt32(1));
                    }

                }
            }
        }
    }
}