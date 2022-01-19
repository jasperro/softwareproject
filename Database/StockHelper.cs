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
        public void AddStockDataToDb(string time, double open, double close, double high, double low, int volume,
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

        public Stock GetStockFromDb(string shortname, DateTimeOffset? day = null)
        {
            var command = DatabaseConnection.CreateCommand();
            ObservableCollection<FinancialPoint> stockPoints = new();
            // Get every first item of every month from database
            command.CommandText = @"
SELECT strftime('%Y-%m-%d %H:%M', max(DateTime) / 1000, 'unixepoch', 'localtime') as DateTime,
       (SELECT Open
        FROM StockData AS T
        WHERE T.DateTime = min(StockData.DateTime)
        GROUP BY strftime('%Y-%m-%d %H:%M', T.DateTime / 1000, 'unixepoch', 'localtime')
       ) AS day_open,
       max(High) AS day_high,
       min(Low) AS day_low,
       (SELECT Close
        FROM StockData AS T
        WHERE T.DateTime = max(StockData.DateTime)
        GROUP BY strftime('%Y-%m-%d %H:%M', T.DateTime / 1000, 'unixepoch', 'localtime')
       ) AS day_close
FROM StockData
WHERE StockShortName = $shortname
GROUP BY strftime('%Y-%m-%d %H:%M', DateTime / 1000, 'unixepoch', 'localtime')
ORDER BY DateTime DESC;
            ";
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