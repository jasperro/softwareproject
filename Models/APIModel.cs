using System;
using System.IO;
using System.Net;

namespace SoftwareProject.Models
{
    /// <summary>
    /// Contains all the methods that can be used to fetch data from the API(s)
    /// </summary>
    public class ApiModel
    {
        // Method to import data from api based on given parameters.
        // String interval must be: daily, 1min, 5min, 15min or 60min.
        public static void DataImport(string ticker, DateTimeOffset date, string interval)
        {
            string function, interval1, slice, datatype;
            string apikey = "VRUNKSO09I7IAXN4";
            DateTimeOffset now = DateTimeOffset.Now;
            TimeSpan difference = now.Subtract(date);
            TimeSpan maxDifference = new TimeSpan(720, 0, 0, 0, 0);
            if (interval == "daily")
            {
                function = "TIME_SERIES_DAILY";
                interval1 = "";
                slice = "";
                datatype = "&datatype=csv";
            }
            else if (difference < maxDifference) 
            {
                    function = "TIME_SERIES_INTRADAY_EXTENDED";
                    interval1 = "&interval=" + interval;
                    int year, month;
                    if (difference.Days <= 360)
                    {
                        year = 1;
                        month = difference.Days / 12;
                    }
                    else
                    {
                        year = 2;
                        month = (difference.Days - 360) / 12;
                    }

                    datatype = "";
                    slice = "&slice=" + "year" + year + "month" + month;
            }
            else 
            {
                function = "";
                interval1 = "";
                slice = "";
                datatype = "";
            }
            
            String url = "https://www.alphavantage.co/query?function=" + function + "&symbol=" + ticker + interval1 + slice +
                         "&apikey=" + apikey + datatype;
            string filename = @"../../../TestData/" + ticker + "/" + ticker + date.Day + "-" + date.Month.ToString() + "-" + date.Year.ToString() +
                              interval + ".csv";
            MakeDirectory(ticker);
            WebClient client = new();
            client.DownloadFile(url, filename);
        }

        private static void MakeDirectory(string name)
        {
            string path = @"../../../TestData/" + name;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
    }
}
