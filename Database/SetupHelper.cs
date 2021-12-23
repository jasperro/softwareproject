using System.Data.Common;
using System.IO;
using Microsoft.Data.Sqlite;
using SoftwareProject.ViewModels;

namespace SoftwareProject
{
    public partial class Database
    {
        
	    public void ImportTestData()
        {
            using (var transaction = DatabaseConnection.BeginTransaction())
            {
                var command = DatabaseConnection.CreateCommand();
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
                        StreamReader sr = new(fs);
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
                    AddStockToDb(
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

        private void SetupDatabase()
        {
            SqliteCommand setupCommand = DatabaseConnection.CreateCommand();
	        setupCommand.CommandText = SetupQuery;
            setupCommand.ExecuteNonQuery();
            ImportTestData();
        }
    }
}