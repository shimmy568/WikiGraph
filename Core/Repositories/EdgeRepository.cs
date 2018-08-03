
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WikiGraph.Core.Repositories
{
    public class EdgeRepository : IEdgeRepository
    {

        public async Task AddEgde(string sourceUrl, string destUrl)
        {
            using (var trans = DataCollector.databaseConnection.BeginTransaction())
            {
                var insertCommand = DataCollector.databaseConnection.CreateCommand();
                insertCommand.Transaction = trans;
                insertCommand.Parameters.AddWithValue("$source", sourceUrl);
                insertCommand.Parameters.AddWithValue("$dest", destUrl);
                await insertCommand.ExecuteNonQueryAsync();
                trans.Commit();
            }
        }

        public async Task AddEdges(IEnumerable<Tuple<string, string>> edges)
        {
            using (var trans = DataCollector.databaseConnection.BeginTransaction())
            {
                foreach (var edge in edges)
                {
                    var insertCommand = DataCollector.databaseConnection.CreateCommand();
                    insertCommand.CommandText = "INSERT INTO Edges ( Source, Dest ) VALUES ( $source, $dest )";
                    insertCommand.Parameters.AddWithValue("$source", edge.Item1);
                    insertCommand.Parameters.AddWithValue("$dest", edge.Item2);
                    await insertCommand.ExecuteNonQueryAsync();
                }
                trans.Commit();
            }
        }

    }
}