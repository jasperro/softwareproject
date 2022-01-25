using System.Collections.ObjectModel;
using System.Linq;
using SoftwareProject.Types;
using SoftwareProject.ViewModels;
using Splat;

namespace SoftwareProject
{
    public static class Globals
    {
        public static Database CurrentDatabase = new();
        
        public static ObservableCollection<Stock> CachedStocks { get; } = new();
        public static MainWindowViewModel MainWindow { get; } = new();

        public static Stock? GetStock(string shortName)
        {
            var stock = CachedStocks.FirstOrDefault(s =>
                s.ShortName == shortName);

            if (stock == null)
            {
                stock = CurrentDatabase.GetStockFromDb(shortName);
                if (stock == null) return null;
                CachedStocks.Add(stock);
            }

            return stock;
        }
        
        public static ILogger Logs = new ConsoleLogger {Level = LogLevel.Debug};
    }
}