using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using Windows.Storage;

namespace VipFit.Database
{
    public class DataAccess
    {
        private static readonly string dbName = "vipfit_sqlite.db";
        private static readonly string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, dbName);

        public async static void InitializeDatabase()
        {
            await ApplicationData.Current.LocalFolder.CreateFileAsync(dbName, CreationCollisionOption.OpenIfExists);

            using var db = new SqliteConnection($"Filename={dbpath}");
            db.Open();

            string tableCommand = "CREATE TABLE IF NOT EXISTS MyTable (Primary_Key INTEGER PRIMARY KEY, Text_Entry NVARCHAR(2048) NULL)";
            var createTable = new SqliteCommand(tableCommand, db);

            createTable.ExecuteReader();
        }

        internal static void AddData(string inputText)
        {
            using var db = new SqliteConnection($"Filename={dbpath}");

            db.Open();

            var insertCommand = new SqliteCommand();
            insertCommand.Connection = db;

            // Use parameterized query to prevent SQL injection attacks
            insertCommand.CommandText = "INSERT INTO MyTable VALUES (NULL, @Entry);";
            insertCommand.Parameters.AddWithValue("@Entry", inputText);

            insertCommand.ExecuteReader();
        }

        internal static IEnumerable<string> GetData()
        {
            using var db = new SqliteConnection($"Filename={dbpath}");

            db.Open();
            var selectCommand = new SqliteCommand("SELECT Text_Entry from MyTable", db);

            SqliteDataReader query = selectCommand.ExecuteReader();

            while (query.Read())
            {
                yield return query.GetString(0);
            }
        }
    }
}
