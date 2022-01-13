using Microsoft.Data.Sqlite;
using SoftwareProject.ViewModels;

namespace SoftwareProject
{
    public partial class Database
    {
        public void NewUser(string name)
        {
            var command = DatabaseConnection.CreateCommand();
            command.CommandText = @"INSERT INTO Users (UserName)
            VALUES($name);";

            command.Parameters.AddWithValue("$name", name);
            command.ExecuteNonQuery();
        }

        public string? GetUsernameFromDb(int userId)
        {
            var command = DatabaseConnection.CreateCommand();
            command.CommandText = @"
			    SELECT * FROM Users WHERE UserId = $userId;	
				";
            command.Parameters.AddWithValue("$userId", userId);
            SqliteDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    return reader.GetString(reader.GetOrdinal("UserName"));
                }
            }

            return null;
        }

        public void CreateTestUser()
        {
            var command = DatabaseConnection.CreateCommand();
            command.CommandText = @"INSERT OR IGNORE INTO Users (UserName, UserId)
            VALUES('TESTUSER', 0);";
            command.ExecuteNonQuery();
        }
    }
}