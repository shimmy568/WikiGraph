

using System.Collections.Generic;
using WikiGraph.Interfaces.Repositories;

namespace WikiGraph.Core.Repositories
{
    public class DataRepairRepository : IDataRepairRepository
    {

        public void RepairUrlStackFromEdges()
        {
            using (var trans = App.databaseConnection.BeginTransaction())
            {
                var selectCommand = App.databaseConnection.CreateCommand();
                selectCommand.Transaction = trans;
                selectCommand.CommandText = @"INSERT INTO UrlStack (Url)
                                              SELECT Dest FROM Edges n
                                              WHERE n.Dest NOT IN(SELECT s.Url FROM UrlStack s WHERE n.Dest = s.Url) AND n.Dest NOT IN(SELECT s.Url FROM Nodes s WHERE n.Dest = s.Url);";

                trans.Commit();
            }
        }

    }
}