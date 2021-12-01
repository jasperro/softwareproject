using System;
using ReactiveUI;
using Microsoft.Data.Sqlite;

namespace SoftwareProject.Models
{
    public class DatabaseModel : ReactiveObject
    {
	    private const string TestQuery = @"
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
		    using var connection = new SqliteConnection("Data Source=database.sqlite");
		    connection.Open();
		    SqliteCommand testCommand = connection.CreateCommand();
		    testCommand.CommandText = TestQuery;
		    testCommand.ExecuteNonQuery();
	    }
    }
}
