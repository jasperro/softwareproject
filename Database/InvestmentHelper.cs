using System.Collections.ObjectModel;
using Microsoft.Data.Sqlite;
using SoftwareProject.Models;
using SoftwareProject.Types;
using SoftwareProject.ViewModels;

namespace SoftwareProject
{
    public partial class Database
    {
        public void AddInvestmentToDb(int userId, Investment investment)
        {
            var command = DatabaseConnection.CreateCommand();
            command.CommandText = @"
				INSERT OR IGNORE INTO Investments
				VALUES ($userId, $shortName, $moneyInvested, 0, $startOfInvestment);
				";

            command.Parameters.AddWithValue("$userId", userId);
            command.Parameters.AddWithValue("$shortName", investment.ShortName);
            command.Parameters.AddWithValue("$moneyInvested", investment.MoneyInvested);
            command.Parameters.AddWithValue("$startOfInvestment", investment.StartOfInvestment);
            command.ExecuteNonQuery();
        }
        public InvestmentPortfolio GetInvestmentPortfolioFromDb(int userId, string? shortName = null)
        {
            var command = DatabaseConnection.CreateCommand();
            InvestmentPortfolio investments = new();
            if (shortName != null)
            {
                command.CommandText = @"
			    SELECT * FROM Investments WHERE UserId = $userid AND ShortName = $shortname;	
				";
                command.Parameters.AddWithValue("$userid", userId);
                command.Parameters.AddWithValue("$shortname", shortName);
            }
            else
            {
                command.CommandText = @"
			    SELECT * FROM Investments WHERE UserId = $userid;	
				";
                command.Parameters.AddWithValue("$userid", userId);
            }

            SqliteDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    investments.Add(new Investment(
                            reader.GetString(reader.GetOrdinal("ShortName")),
                            reader.GetDateTime(reader.GetOrdinal("StartOfInvestment"))
                        )
                    );
                }
            }

            reader.Close();
            return investments;
        }
    }
}