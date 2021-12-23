using System;
using System.Collections.ObjectModel;
using LiveChartsCore.Defaults;
using Microsoft.Data.Sqlite;
using SoftwareProject.Types;
using SoftwareProject.ViewModels;

namespace SoftwareProject
{
    public partial class Database
    {
        public void AddStockToDb(string time, double open, double close, double high, double low, int volume,
            string shortname)
        {
            var command = DatabaseConnection.CreateCommand();
            command.CommandText = @"
				INSERT OR IGNORE INTO StockData
				VALUES ($open, $close, $volume, $high, $low, $shortname, $time)
				";

            command.Parameters.AddWithValue("$open", open);
            command.Parameters.AddWithValue("$close", close);
            command.Parameters.AddWithValue("$volume", volume);
            command.Parameters.AddWithValue("$high", high);
            command.Parameters.AddWithValue("$low", low);
            command.Parameters.AddWithValue("$shortname", shortname);
            command.Parameters.AddWithValue("$time", time);
            command.ExecuteNonQuery();
        }

        public Stock GetStockFromDb(string shortname)
        {
            var command = DatabaseConnection.CreateCommand();
            ObservableCollection<FinancialPoint> stockPoints = new();
            command.CommandText = @"
			    SELECT * FROM StockData WHERE StockShortName = $shortname;	
				";
            command.Parameters.AddWithValue("$shortname", shortname);
            SqliteDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    stockPoints.Add(new StockPoint(
                        reader.GetDateTime(reader.GetOrdinal("DateTime")),
                        reader.GetDouble(reader.GetOrdinal("High")),
                        reader.GetDouble(reader.GetOrdinal("Open")),
                        reader.GetDouble(reader.GetOrdinal("Close")),
                        reader.GetDouble(reader.GetOrdinal("Low")
                        )
                    ));
                }
            }
            else
            {
                Console.WriteLine("No rows found.");
            }

            reader.Close();
            return new Stock(shortname, stockPoints);
        }
        
        
        public void FollowStock(int id, string stock)
        {
            var command = DatabaseConnection.CreateCommand();
            command.CommandText = @"
            INSERT INTO FollowedStocks (UserId, ShortName)
            VALUES ($id, $stock);";

            command.Parameters.AddWithValue("$id", id);
            command.Parameters.AddWithValue("$stock", stock);
            command.ExecuteNonQuery();
        }
    }
}