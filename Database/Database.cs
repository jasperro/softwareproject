using Microsoft.Data.Sqlite;

namespace SoftwareProject
{
    public partial class Database
    {
        private SqliteConnection DatabaseConnection { get; }
        public Database()
        {
             DatabaseConnection = new SqliteConnection("Data Source=../../../database.sqlite");
             DatabaseConnection.Open();
             SetupDatabase();
        }
    }
}