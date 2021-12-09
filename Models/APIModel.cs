using System;
using System.IO;
using System.Net;

namespace SoftwareProject.Models
{
    public class APIModel
    {
        public void DataImport(string ticker, DateTime date, string interval)
        {
            string function, interval1, slice, datatype;
            string apikey = "VRUNKSO09I7IAXN4";
            DateTime now = DateTime.Now;
            TimeSpan difference = now.Subtract(date);
            TimeSpan maxDifference = new TimeSpan(720, 0, 0, 0, 0);
            if (interval == "daily")
            {
                function = "TIME_SERIES_DAILY";
                interval1 = "";
                slice = "";
                datatype = "datatype=csv";
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
                    slice = "&slice=" + "year" + year.ToString() + "month" + month.ToString();
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
            string filename = @"softwareproject\TestData\" + ticker + "\\" + ticker + date.Day.ToString() + "-" + date.Month.ToString() + "-" + date.Year.ToString() +
                              interval;
            MakeDirectory(ticker);
            WebClient client = new WebClient();
            client.DownloadFile(url, filename);
            
            
        }

        private void MakeDirectory(string name)
        {
            string path = @"softwareproject\TestData\" + name;
            if (!(Directory.Exists(path)))
            {
                Directory.CreateDirectory(path);
            }
        }
    }
}
