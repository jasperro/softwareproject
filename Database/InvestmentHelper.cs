using System;
using Microsoft.Data.Sqlite;
using SoftwareProject.Types;

namespace SoftwareProject
{
    public partial class Database
    {
        public void SellInvestment(int userId, Investment investment)
        {
            var command = DatabaseConnection.CreateCommand();
            command.CommandText = @"
				DELETE FROM Investments WHERE StartOfInvestment = $startOfInvestment
				";

            command.Parameters.AddWithValue("$startOfInvestment", investment.StartOfInvestment);
            command.ExecuteNonQuery();
        }
        
        
        public void AddInvestmentToDb(int userId, Investment investment)
        {
            var command = DatabaseConnection.CreateCommand();
            command.CommandText = @"
				INSERT OR IGNORE INTO Investments
				VALUES ($userId, $shortName, $amountInvested, $moneyInvested, 0, $startOfInvestment);
				";

            command.Parameters.AddWithValue("$userId", userId);
            command.Parameters.AddWithValue("$shortName", investment.ShortName);
            command.Parameters.AddWithValue("$amountInvested", investment.AmountInvested);
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
                    try
                    {
                        investments.Add(new Investment(
                            reader.GetString(reader.GetOrdinal("ShortName")),
                            reader.GetDouble(reader.GetOrdinal("MoneyInvested")), 
                            (int)reader.GetInt64(reader.GetOrdinal("AmountInvested")),
                            reader.GetDateTime(reader.GetOrdinal("StartOfInvestment")))
                        );
                    }
                    catch (InvalidOperationException)
                    {
                        // Stock is not in the database
                    }
                }
            }

            reader.Close();
            return investments;
        }
    }
}