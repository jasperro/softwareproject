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

        public void CreateTestUser()
        {
            var command = DatabaseConnection.CreateCommand();
            command.CommandText = @"INSERT OR IGNORE INTO Users (UserName, UserId)
            VALUES('TESTUSER', 0);";
            command.ExecuteNonQuery();
        }
    }
}