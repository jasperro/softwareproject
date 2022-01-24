using Microsoft.Data.Sqlite;
using System;
using SoftwareProject.Types;

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
            VALUES('TESTUSER', 0);
            INSERT OR IGNORE INTO UserSettings (UserId) VALUES(0)";
            command.ExecuteNonQuery();
        }

        public void UpdateSettingsToDb(int userId, Usersettings settings)
        {
            var command = DatabaseConnection.CreateCommand();
            command.CommandText = @"
				UPDATE UserSettings
				SET SimTime = $SimTime
                WHERE UserId = $userId
				";

            command.Parameters.AddWithValue("$userId", userId);
            command.Parameters.AddWithValue("$SimTime", settings.SimTime);
            command.ExecuteNonQuery();
        }


        public Usersettings GetSettingsFromUserSettingsDb(int userId = 0)
        {
            var command = DatabaseConnection.CreateCommand();

            int? tijd = null;
            
            command.CommandText = @"
			    SELECT * FROM UserSettings WHERE UserId = $userid;	
				";
            command.Parameters.AddWithValue("$userid", userId);

            SqliteDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    int tijdOrdinal = reader.GetOrdinal("SimTime");
                    tijd = reader.IsDBNull(tijdOrdinal) ? null : int.Parse(reader.GetString(tijdOrdinal));
                }
            }

            reader.Close();

            Usersettings settings = new(tijd);
            return settings;
        }
    }
}