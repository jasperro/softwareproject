using System;
using System.IO;
using System.Linq;
using System.Net;
using static SoftwareProject.ViewModels.MainWindowViewModel;

namespace SoftwareProject.Models
{
    /// <summary>
    /// Contains all the methods that can be used to fetch data from the API(s)
    /// </summary>
    public class ApiModel
    {
        public enum ImportType
        {
            Stock = 0,
            Crypto = 1
        }

        /// <summary>
        /// Method to import data from api based on given parameters.
        /// </summary>
        /// <example>
        /// String interval must be: daily for daily data, or
        /// 1min, 5min, 15min or 60min for intra-day.
        /// </example>
        public static string DataImport(string ticker, DateTimeOffset date, string interval = "daily",
            ImportType type = ImportType.Crypto)
        {
            string parameters = "";

            string function = type == ImportType.Stock ? "TIME_SERIES_DAILY" : "DIGITAL_CURRENCY_DAILY";
            string market = type == ImportType.Crypto ? "&market=USD" : "";
            string datatype = "&datatype=csv";
            string apikey = User.ApiKey;

            TimeSpan difference = DateTimeOffset.Now.Subtract(date);
            TimeSpan maxDifference = new TimeSpan(720, 0, 0, 0, 0);

            if (difference < maxDifference)
            {
                function = type == ImportType.Stock ? "TIME_SERIES_INTRADAY_EXTENDED" : "CRYPTO_INTRADAY";
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

                parameters = $"&interval={interval}";
                if (type == ImportType.Stock)
                {
                    parameters += $"&slice=year{year}month{month}";
                }
            }

            string url =
                $"https://www.alphavantage.co/query?function={function}&symbol={ticker}&apikey={apikey}{datatype}{parameters}{market}";
            string filename =
                $@"../../../TestData/{ticker}/{ticker}{date.Day}-{date.Month}-{date.Year}-{interval}-{DateTime.Now.ToShortTimeString()}.csv";
            MakeDirectory(ticker);
            WebClient client = new();
            client.DownloadFile(url, filename);
            return filename;
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