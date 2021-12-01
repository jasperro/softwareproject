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
