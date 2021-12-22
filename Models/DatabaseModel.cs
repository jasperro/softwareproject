using System;
using System.Collections.ObjectModel;
using System.IO;
using LiveChartsCore.Defaults;
using ReactiveUI;
using Microsoft.Data.Sqlite;
using SoftwareProject.Types;

namespace SoftwareProject.Models
{
    /// <summary>
    /// Stores the database and methods that modify and read data from it
    /// </summary>
    public class DatabaseModel : ReactiveObject
    {
        public SqliteConnection DbConnection { get; } = new("Data Source=../../../database.sqlite");

        public void ImportTestData()
        {
            using (var transaction = DbConnection.BeginTransaction())
            {
                var command = DbConnection.CreateCommand();
                command.CommandText = @"
				INSERT OR IGNORE INTO Stocks
				VALUES ($shortname, $longname)
				";

                var shortnameParameter = command.CreateParameter();
                var longnameParameter = command.CreateParameter();

                shortnameParameter.ParameterName = "$shortname";
                longnameParameter.ParameterName = "$longname";

                command.Parameters.Add(shortnameParameter);
                command.Parameters.Add(longnameParameter);


                string[] stockpaths = Directory.GetDirectories("../../../TestData");

                foreach (var path in stockpaths)
                {
                    DirectoryInfo d = new(path);
                    var splitdata = d.Name.Split(", ");
                    string shortname = splitdata[0];
                    try
                    {
                        longnameParameter.Value = splitdata[1];
                    }
                    catch
                    {
                        longnameParameter.Value = splitdata[0];
                    }

                    shortnameParameter.Value = shortname;
                    command.ExecuteNonQuery();
                    FileInfo[] datafiles = d.GetFiles("*.csv");
                    foreach (var df in datafiles)
                    {
                        FileStream fs = df.OpenRead();
                        StreamReader sr = new StreamReader(fs);
                        ParseCsv(sr, shortname);
                    }
                }

                transaction.Commit();
            }
        }

        private void ParseCsv(StreamReader sr, string shortname)
        {
            string? line;
            var currentLine = 0;
            while ((line = sr.ReadLine()) != null)
            {
                string[] csvLine = line.Split(',');

                if (currentLine != 0)
                {
                    AddStockDataToDb(
                        csvLine[0],
                        double.Parse(csvLine[1]),
                        double.Parse(csvLine[2]),
                        double.Parse(csvLine[3]),
                        double.Parse(csvLine[4]),
                        int.Parse(csvLine[5]),
                        shortname
                    );
                }

                currentLine++;
            }
        }

        private void AddStockDataToDb(string time, double open, double close, double high, double low, int volume,
            string shortname)
        {
            var command = DbConnection.CreateCommand();
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

        private const string SetupQuery = @"
		CREATE TABLE IF NOT EXISTS Stocks ( 
			ShortName            varchar(100) NOT NULL  PRIMARY KEY  ,
			LongName             varchar(100) NOT NULL    
		);

		CREATE TABLE IF NOT EXISTS Users ( 
			UserName             varchar(100) NOT NULL    ,
			UserId               integer NOT NULL  PRIMARY KEY  
		);

		CREATE TABLE IF NOT EXISTS FollowedStocks ( 
			UserId               integer NOT NULL    ,
			ShortName            varchar(100)     ,
			Status               char(1)     ,
			FOREIGN KEY ( UserId ) REFERENCES Users( UserId )  ,
			FOREIGN KEY ( ShortName ) REFERENCES Stocks( ShortName )  
		);

		CREATE TABLE IF NOT EXISTS Investments ( 
			UserId               integer NOT NULL    ,
			ShortName            varchar(100) NOT NULL    ,
			MoneyInvested        double NOT NULL    ,
			MoneyReturn			 double		,
			StartOfInvestment	 datetime NOT NULL ,
			FOREIGN KEY ( UserId ) REFERENCES Users( UserId )  ,
			FOREIGN KEY ( ShortName ) REFERENCES Stocks( ShortName )  
		);
        
        CREATE TABLE IF NOT EXISTS UserSettings ( 
			UserId               integer NOT NULL    ,			
			FOREIGN KEY ( UserId ) REFERENCES Users( UserId )			  
		);

		CREATE TABLE IF NOT EXISTS StockData ( 
			Open                 double     ,
			Close                double     ,
			Volume               integer     ,
			High                 double     ,
			Low                  double     ,
			StockShortName       varchar(100) NOT NULL    ,
			DateTime             datetime NOT NULL ,
			PRIMARY KEY ( StockShortName, DateTime ),
			FOREIGN KEY ( StockShortName ) REFERENCES Stocks( ShortName )  
		);
		";

        public Stock GetStockFromDb(string shortname)
        {
            var command = DbConnection.CreateCommand();
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

        public ObservableCollection<Investment> GetInvestmentsFromDb(int userId, string? shortName = null)
        {
            var command = DbConnection.CreateCommand();
            ObservableCollection<Investment> investments = new();
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
                            reader.GetDateTime(reader.GetOrdinal("High"))
                        )
                    );
                }
            }

            reader.Close();
            return investments;
        }

        public void NewUser(string name)
        {
            var command = DbConnection.CreateCommand();
            command.CommandText = @"INSERT INTO Users (UserName)
            VALUES($name);";

            command.Parameters.AddWithValue("$name", name);
            command.ExecuteNonQuery();
        }

        public void FollowStock(int id, string stock)
        {
            var command = DbConnection.CreateCommand();
            command.CommandText = @"
            INSERT INTO FollowedStocks (UserId, ShortName)
            VALUES ($id, $stock);";

            command.Parameters.AddWithValue("$id", id);
            command.Parameters.AddWithValue("$stock", stock);
            command.ExecuteNonQuery();
        }

        public DatabaseModel()
        {
            DbConnection.Open();
            SqliteCommand setupCommand = DbConnection.CreateCommand();
            setupCommand.CommandText = SetupQuery;
            setupCommand.ExecuteNonQuery();
            ImportTestData();
        }
    }
}