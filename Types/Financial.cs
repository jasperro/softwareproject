using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SoftwareProject.ViewModels;

namespace SoftwareProject.Types
{
    public class StockPoint : FinancialPoint
    {
        public StockPoint(DateTime date, double high, double open, double close, double low, int volume = 0) : base(
            date, high, open, close, low)
        {
            Volume = volume;
        }

        public int Volume { get; set; }
    }

    public class Stock : CandlesticksSeries<FinancialPoint>, IStock
    {
        [Reactive]
        public ObservableCollection<FinancialPoint> AllValues { get; set; }

        public string LongName { get; set; } = "Abcd efghi";

        public string ShortName
        {
            get => Name;
            set => Name = value;
        }

        public DateTime LastUpdate => Values != null ? Values.Last().Date : DateTime.Now;

        public double TrendPercentage => 0;

        public Stock(string shortName = "ABCD", ObservableCollection<FinancialPoint>? defaultData = null)
        {
            // Set default values if stock has no data yet.
            AllValues = defaultData ?? new ObservableCollection<FinancialPoint>();

            ShortName = shortName;

            MainWindowViewModel.GlobalData.Timer.Subscribe(x => Values = AllValues.Where(financialPoint => financialPoint.Date.CompareTo(MainWindowViewModel.GlobalData.CurrentTime) < 0));
        }

        /// <summary>Update all stocks data to match current application time</summary>
        public void UpdateToTime(DateTime currentTime)
        {
        }
    }

    public class Investment
    {
        /// <summary>This list contains the predictive algorithms that will be applied after each other</summary>
        /// <example>This allows us to take for example the average, and apply a smoothing factor after this.</example>
        public List<IAlgorithm> AlgorithmList;
        public Stock Stock { get; }

        public Investment(Stock stock, DateTime? startOfInvestment = null)
        {
            Stock = stock;
            StartOfInvestment = startOfInvestment ?? DateTime.Now;
        }
        public Investment(string shortName, DateTime? startOfInvestment = null)
        {
            Stock = MainWindowViewModel.GlobalData.AvailableStocks.FirstOrDefault(x => x.ShortName == shortName) ?? Globals.CurrentDatabase.GetStockFromDb(shortName);
            StartOfInvestment = startOfInvestment ?? DateTime.Now;
        }

        public string ShortName
        {
            get => Stock.ShortName;
            set => Stock.ShortName = value;
        }

        public DateTime StartOfInvestment { get; }
        public double Profit => MoneyReturn - MoneyInvested;
        public double MoneyInvested { get; set; }
        public double MoneyReturn { get; set; }
    }

    public interface IStock : ISeries<FinancialPoint>
    {
        public string ShortName { get; set; }
        public string LongName { get; set; }
    }
}