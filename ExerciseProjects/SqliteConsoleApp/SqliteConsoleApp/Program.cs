using Microsoft.Data.Sqlite;

namespace SqliteConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            SqliteConnectionStringBuilder scsb = new SqliteConnectionStringBuilder
            {
                DataSource = "ADO.sqlite",
                Mode = SqliteOpenMode.ReadWriteCreate,
                //Password = "MyEncryptionKey",
            };

            using (var connection = new SqliteConnection(scsb.ConnectionString))
            {
                connection.Open();
                var createTableCommand = connection.CreateCommand();
                createTableCommand.CommandText = @"
CREATE TABLE IF NOT EXISTS Inventory
(
    ID INTEGER PRIMARY KEY AUTOINCREMENT,
    Name TEXT NOT NULL,
    Price TEXT NOT NULL,
    Quantity INTEGER NOT NULL,
    AddedOn TEXT NOT NULL DEFAULT CURRENT_TIMESTAMP
);
";
                createTableCommand.ExecuteNonQuery();
            }
        }
    }
}
