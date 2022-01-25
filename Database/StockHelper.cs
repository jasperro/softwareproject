using System;
using System.Collections.ObjectModel;
using System.Globalization;
using LiveChartsCore.Defaults;
using Microsoft.Data.Sqlite;
using SoftwareProject.Types;

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

        public Stock? GetStockFromDb(string shortname, DateTimeOffset? day = null)
        {
            var command = DatabaseConnection.CreateCommand();
            ObservableCollection<FinancialPoint> stockPoints = new();
            command.Parameters.AddWithValue("$shortname", shortname);
            // Get daily data from intraday
            if (day == null)
                command.CommandText = @"
                SELECT max(High) as High,
                       min(Low) as Low,
                       (SELECT Open
                        FROM StockData AS T
                        WHERE T.DateTime = min(StockData.DateTime) AND T.StockShortName = $shortname
                        GROUP BY T.DateTime
                       ) AS Open,
                       (SELECT Close
                        FROM StockData AS T
                        WHERE T.DateTime = max(StockData.DateTime) AND T.StockShortName = $shortname
                        GROUP BY T.DateTime
                       ) AS Close,
                       max(DateTime) as DateTime
                FROM StockData
                WHERE StockShortName = $shortname
                GROUP BY strftime('%Y-%m-%d', DateTime);
                ";
            else
            {
                // Get intraday data
                command.CommandText = @"
			    SELECT * FROM StockData
			    WHERE StockShortName = $shortname
                AND strftime('%Y-%m-%d', DateTime) = $day;
			    ";
                command.Parameters.AddWithValue("$day", day.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
            }

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

                reader.Close();
                return new Stock(shortname, stockPoints);
            }

            reader.Close();
            Console.WriteLine("No rows found.");
            return null;
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