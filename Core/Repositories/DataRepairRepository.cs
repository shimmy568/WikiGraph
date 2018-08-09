

using System.Collections.Generic;
using System.Threading.Tasks;
using WikiGraph.Interfaces.Repositories;

namespace WikiGraph.Core.Repositories
{
    public class DataRepairRepository : IDataRepairRepository
    {

        public async Task RepairUrlStackFromEdges()
        {
            using (var trans = App.databaseConnection.BeginTransaction())
            {
                var selectCommand = App.databaseConnection.CreateCommand();
                selectCommand.Transaction = trans;
                selectCommand.CommandText = @"INSERT INTO UrlStack (Url)
                                              SELECT Dest FROM Edges n
                                              WHERE n.Dest NOT IN(SELECT s.Url FROM UrlStack s WHERE n.Dest = s.Url) AND n.Dest NOT IN(SELECT s.Url FROM Nodes s WHERE n.Dest = s.Url);";
                await selectCommand.ExecuteNonQueryAsync();
                trans.Commit();
            }
        }

        public IEnumerable<string> GetAllHtmlForNodes()
        {
            using (var trans = App.databaseConnection.BeginTransaction())
            {
                var selectCommand = App.databaseConnection.CreateCommand();
                selectCommand.Transaction = trans;
                selectCommand.CommandText = "SELECT Html FROM Nodes;";

                using (var reader = selectCommand.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        yield return reader.GetString(0);
                    }

                }
                trans.Commit();
            }
        }

    }
}