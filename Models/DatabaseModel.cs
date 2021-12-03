using System;
using System.IO;
using ReactiveUI;
using Microsoft.Data.Sqlite;

namespace SoftwareProject.Models
{
    public class DatabaseModel : ReactiveObject
    {
        private SqliteConnection DbConnection { get; } = new SqliteConnection($"Data Source=../../../database.sqlite");

        public void ImportTestData()
        {
            using (var transaction = DbConnection.BeginTransaction())
            {
                var command = DbConnection.CreateCommand();
                command.CommandText = @"
				INSERT OR IGNORE INTO Stocks
				VALUES ($shortname, $longname)
				";

                var shortname = command.CreateParameter();
                var longname = command.CreateParameter();

                shortname.ParameterName = "$shortname";
                longname.ParameterName = "$longname";

                command.Parameters.Add(shortname);
                command.Parameters.Add(longname);

                string[] stockpaths = Directory.GetDirectories("../../../TestData");

                foreach (var path in stockpaths)
                {
                    DirectoryInfo d = new(path);
                    (shortname.Value, longname.Value) =
                        d.Name.Split(", ")
                            switch { var dn => (dn[0], dn[1]) };
                }

                command.ExecuteNonQuery();

                transaction.Commit();
            }
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
			DateTime             datetime     ,
			FOREIGN KEY ( StockShortName ) REFERENCES Stocks( ShortName )  
		);
		";

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