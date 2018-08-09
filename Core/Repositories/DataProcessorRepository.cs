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
            using (var trans = App.databaseConnection.BeginTransaction())
            {
                var selectCommand = App.databaseConnection.CreateCommand();
                selectCommand.Transaction = trans;
                selectCommand.CommandText = "SELECT b.Title, Dest, COUNT(*) total FROM Edges a INNER JOIN Nodes b ON a.Dest = b.Url GROUP BY Dest ORDER BY total DESC;";

                var urls = new List<string>();
                using (var reader = selectCommand.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        yield return new UrlCountPair(reader.GetString(0), reader.GetString(1), reader.GetInt32(2));
                    }

                }
                trans.Commit();
            }
        }
    }
}