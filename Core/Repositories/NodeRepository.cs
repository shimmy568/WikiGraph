using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using WikiGraph.Core.Models;
using WikiGraph.Interfaces.Repositories;
using System.Collections.Generic;

namespace WikiGraph.Core.Repositories
{
    public class NodeRepository : INodeRepository
    {

        private readonly string PropOrder = "rowid, Url, Html, TimeRetrieved, Title ";

        // TODO: Create method to create node from reader to avoid code duplication
        public async Task InsertNewNode(string url, string html, DateTime timeRetrieved, string title)
        {
            using (var trans = App.databaseConnection.BeginTransaction())
            {
                var insertCommand = App.databaseConnection.CreateCommand();
                insertCommand.Transaction = trans;
                insertCommand.CommandText = "INSERT INTO Nodes ( Url, Html, TimeRetrieved, Title ) VALUES ( $url, $html, $time, $title )";
                insertCommand.Parameters.AddWithValue("$url", url);
                insertCommand.Parameters.AddWithValue("$html", html);
                insertCommand.Parameters.AddWithValue("$title", title);
                insertCommand.Parameters.AddWithValue("$time", timeRetrieved);
                await insertCommand.ExecuteNonQueryAsync();

                trans.Commit();
            }
        }

        public async Task<Node> GetNodeByID(int id)
        {
            using (var trans = App.databaseConnection.BeginTransaction())
            {
                var selectCommand = App.databaseConnection.CreateCommand();
                selectCommand.Transaction = trans;
                selectCommand.CommandText = $"SELECT {PropOrder} FROM Nodes WHERE rowid = $id";
                selectCommand.Parameters.AddWithValue("$id", id);
                using (var reader = selectCommand.ExecuteReader())
                {
                    if (!await reader.ReadAsync())
                    {
                        return null;
                    }

                    var node = GetNodeFromReader(reader);

                    trans.Commit();
                    return node;
                }
            }
        }

        public async Task<Node> GetNodeByUrl(string url)
        {
            using (var trans = App.databaseConnection.BeginTransaction())
            {
                var selectCommand = App.databaseConnection.CreateCommand();
                selectCommand.Transaction = trans;
                selectCommand.CommandText = $"SELECT {PropOrder} FROM Nodes WHERE Url = $url";
                selectCommand.Parameters.AddWithValue("$url", url);
                using (var reader = selectCommand.ExecuteReader())
                {
                    // If there is nothing to read return null
                    if (!await reader.ReadAsync())
                    {
                        return null;
                    }

                    var node = GetNodeFromReader(reader);

                    trans.Commit();
                    return node;
                }
            }
        }

        /// To use this the sql data must be queried in the proper order
        private Node GetNodeFromReader(SqliteDataReader reader)
        {
            var node = new Node
            {
                NodeID = reader.GetInt32(0),
                Url = reader.GetString(1),
                Html = reader.GetString(2),
                TimeRetrieved = DateTime.Parse(reader.GetString(3)),
                Title = reader.GetString(4)
            };
            return node;
        }

        public IEnumerable<Node> GetNodesByUrls(IEnumerable<string> urls)
        {
            using (var trans = App.databaseConnection.BeginTransaction())
            {
                var selectCommand = App.databaseConnection.CreateCommand();
                selectCommand.Transaction = trans;
                selectCommand.CommandText = $"SELECT {PropOrder} FROM Nodes WHERE Url IN ($urls)";
                selectCommand.Parameters.AddWithValue("$urls", urls);

                using (var reader = selectCommand.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        yield return GetNodeFromReader(reader);
                    }

                }
                trans.Commit();
            }
        }
    }
}