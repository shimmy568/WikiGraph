using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using WikiGraph.Interfaces.Repositories;

namespace WikiGraph.Core.Repositories
{
    public class UrlStackRepository : IUrlStackRepository
    {
        public async Task<List<string>> GetUrlsFromStack()
        {
            using (var trans = App.databaseConnection.BeginTransaction())
            {
                var selectCommand = App.databaseConnection.CreateCommand();
                selectCommand.Transaction = trans;
                selectCommand.CommandText = "SELECT Url FROM UrlStack ORDER BY rowid ASC LIMIT 5000;";

                var urls = new List<string>();
                using (var reader = selectCommand.ExecuteReader())
                {

                    while (await reader.ReadAsync())
                    {
                        urls.Add(reader.GetString(0));
                    }

                }

                var deleteCommand = App.databaseConnection.CreateCommand();
                deleteCommand.CommandText = "DELETE FROM UrlStack WHERE Url IN (SELECT Url FROM UrlStack ORDER BY rowid ASC LIMIT 5000);";

                await deleteCommand.ExecuteNonQueryAsync();

                trans.Commit();

                return urls;
            }
        }

        public async Task AddUrlToStack(string url)
        {
            using (var trans = App.databaseConnection.BeginTransaction())
            {
                var insertCommand = App.databaseConnection.CreateCommand();
                insertCommand.Transaction = trans;
                insertCommand.CommandText = "INSERT INTO UrlStack ( Url ) VALUES ( $url )";
                insertCommand.Parameters.AddWithValue("$url", url);
                await insertCommand.ExecuteNonQueryAsync();
            }
        }

        public async Task AddUrlsToStack(IEnumerable<string> urls)
        {
            using (var trans = App.databaseConnection.BeginTransaction())
            {
                foreach (var url in urls)
                {
                    var insertCommand = App.databaseConnection.CreateCommand();
                    insertCommand.CommandText = "INSERT INTO UrlStack ( Url ) VALUES ( $url )";
                    insertCommand.Parameters.AddWithValue("$url", url);
                    await insertCommand.ExecuteNonQueryAsync();
                }
                trans.Commit();
            }
        }

        public async Task<int> CountRowsInStack()
        {
            using (var trans = App.databaseConnection.BeginTransaction())
            {
                var selectCommand = App.databaseConnection.CreateCommand();
                selectCommand.Transaction = trans;
                selectCommand.CommandText = "SELECT COUNT(*) FROM UrlStack;";
                using (var reader = selectCommand.ExecuteReader())
                {
                    await reader.ReadAsync();

                    var count = reader.GetInt32(0);

                    trans.Commit();
                    return count;
                }
            }
        }

        public async Task<bool> IsUrlInDatabaseStack(string url)
        {
            using (var trans = App.databaseConnection.BeginTransaction())
            {
                var selectCommand = App.databaseConnection.CreateCommand();
                selectCommand.Transaction = trans;
                selectCommand.CommandText = "SELECT COUNT(*) FROM UrlStack WHERE Url = $url;";
                selectCommand.Parameters.AddWithValue("$url", url);
                using (var reader = selectCommand.ExecuteReader())
                {
                    await reader.ReadAsync();

                    var count = reader.GetInt32(0);

                    trans.Commit();
                    return count == 1;
                }
            }
        }
    }
}