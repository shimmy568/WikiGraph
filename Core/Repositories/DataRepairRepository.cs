
using System.Linq;
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
                                              SELECT DISTINCT Dest FROM Edges n
                                              WHERE n.Dest NOT IN(SELECT s.Url FROM UrlStack s WHERE n.Dest = s.Url) AND n.Dest NOT IN(SELECT s.Url FROM Nodes s WHERE n.Dest = s.Url);";
                await selectCommand.ExecuteNonQueryAsync();
                trans.Commit();
            }
        }

        public IEnumerable<string> GetAllHtmlForNodes()
        {
            long count;
            using (var trans = App.databaseConnection.BeginTransaction())
            {
                var selectCommand = App.databaseConnection.CreateCommand();
                selectCommand.Transaction = trans;
                selectCommand.CommandText = "SELECT COUNT(*) FROM Nodes";
                using (var reader = selectCommand.ExecuteReader())
                {
                    reader.Read();

                    count = reader.GetInt64(0);
                    trans.Commit();
                }
            }

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

        public IEnumerable<string> FilterUrlsThatAppearInDatabase(IEnumerable<string> urls)
        {
            using (var trans = App.databaseConnection.BeginTransaction())
            {
                var selectCommand1 = App.databaseConnection.CreateCommand();
                selectCommand1.Transaction = trans;
                selectCommand1.CommandText = "SELECT Url FROM Nodes WHERE Url IN ($urls);";
                selectCommand1.Parameters.AddWithValue("$urls", urls.ToString());

                using (var reader = selectCommand1.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        yield return reader.GetString(0);
                    }

                }

                var selectCommand2 = App.databaseConnection.CreateCommand();
                selectCommand2.Transaction = trans;
                selectCommand2.CommandText = "SELECT Url FROM UrlStack WHERE Url IN ($urls);";
                selectCommand2.Parameters.AddWithValue("$urls", urls.ToString());

                using (var reader = selectCommand2.ExecuteReader())
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