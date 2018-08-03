
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WikiGraph.Core.Models;
using Microsoft.Data.Sqlite;

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

        public IEnumerable<Edge> GetEdgesByDestinationUrl(string url)
        {
            using (var trans = DataCollector.databaseConnection.BeginTransaction())
            {
                var selectCommand = DataCollector.databaseConnection.CreateCommand();
                selectCommand.Transaction = trans;
                selectCommand.CommandText = "SELECT Source, Dest FROM Edges WHERE Dest = $dest;";
                selectCommand.Parameters.AddWithValue("$dest", url);

                var urls = new List<string>();
                using (var reader = selectCommand.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        yield return EdgeFromReader(reader);
                    }

                }
            }
        }

        private Edge EdgeFromReader(SqliteDataReader reader)
        {
            return new Edge{
                SourceUrl = reader.GetString(0),
                DestinationUrl = reader.GetString(1)
            };
        }
    }
}