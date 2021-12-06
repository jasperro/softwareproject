using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using DynamicData.Kernel;
using LiveChartsCore.Defaults;
using ReactiveUI;
using Microsoft.Data.Sqlite;
using SoftwareProject.ViewModels;

namespace SoftwareProject.Models
{
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
                    string shortname;
                    (shortname, longnameParameter.Value) =
                        d.Name.Split(", ")
                            switch { var dn => (dn[0], dn[1]) };
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

        private Stock GetStockFromDb(string shortname)
        {
            var command = DbConnection.CreateCommand();
            ObservableCollection<FinancialPoint>? stockPoints = new();
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

        public DatabaseModel()
        {
            DbConnection.Open();
            SqliteCommand setupCommand = DbConnection.CreateCommand();
            setupCommand.CommandText = SetupQuery;
            setupCommand.ExecuteNonQuery();
            ImportTestData();
            MainWindowViewModel.HomePage.Series.Add(GetStockFromDb("AAPL"));
        }
    }
}