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
        public static void DataImport(string ticker, string interval = "daily",
            ImportType type = ImportType.Stock, int downloadMonths = 1, int startDownloadMonthsAgo = 23)
        {
            string intervalParameters = "";
            string sliceParameters = "";

            string function = type == ImportType.Stock ? "TIME_SERIES_DAILY" : "DIGITAL_CURRENCY_DAILY";
            string market = type == ImportType.Crypto ? "&market=USD" : "";
            string datatype = "&datatype=csv";
            string apikey = User.ApiKey;

            if (interval != "daily")
            {
                function = type == ImportType.Stock ? "TIME_SERIES_INTRADAY_EXTENDED" : "CRYPTO_INTRADAY";
                intervalParameters = $"&interval={interval}";
            }
            
            int counter = 0;

            void SetSliceParameters()
            {
                int year = (startDownloadMonthsAgo - counter)/12 + 1;
                int month = (startDownloadMonthsAgo - counter)%12 + 1;

                if (type == ImportType.Stock)
                {
                    sliceParameters = $"&slice=year{year}month{month}";
                }
            }
            
            do
            {
                if (interval != "daily")
                {
                    SetSliceParameters();
                }

                if (startDownloadMonthsAgo < downloadMonths) return;
                string url =
                    $"https://www.alphavantage.co/query?function={function}&symbol={ticker}&apikey={apikey}{datatype}{intervalParameters}{sliceParameters}{market}";
                string filename =
                    $@"../../../TestData/{ticker}/{ticker}-{interval}-{24 - startDownloadMonthsAgo + counter}.csv";
                MakeDirectory(ticker);
                WebClient client = new();
                client.DownloadFile(url, filename);
                Console.WriteLine($"Downloaded {filename}");
                counter++;
            } while (counter < downloadMonths);
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