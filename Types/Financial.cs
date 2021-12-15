using System;
using System.Collections.ObjectModel;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;

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
        private readonly ObservableCollection<FinancialPoint> _observableValues;

        public string LongName { get; set; } = "Abcd efghi";

        public string ShortName
        {
            get => Name;
            set => Name = value;
        }

        public Stock(string shortName = "ABCD", ObservableCollection<FinancialPoint>? defaultData = null)
        {
            // Set default values if stock has no data yet.
            _observableValues = defaultData ?? new ObservableCollection<FinancialPoint>
            {
            };

            Values = _observableValues;
            ShortName = shortName;
        }
    }

    public class Investment
    {
        private Stock Stock { get; }

        public Investment(Stock? stock = null, DateTime? startOfInvestment = null)
        {
            Stock = stock ?? new Stock(DateTime.Now.ToString());
            StartOfInvestment = startOfInvestment ?? DateTime.Now;
        }

        public string ShortName
        {
            get => Stock.ShortName;
            set => Stock.ShortName = value;
        }

        public DateTime StartOfInvestment { get; }
        public DateTime LastUpdate { get; set; }
        public double TrendPercentage { get; set; }
        public double Profit => MoneyReturn - MoneyInvested;
        public double MoneyInvested { get; set; }
        public double MoneyReturn { get; set; }
    }

    /*public class Investment
    {
        public string ShortName { get => "ABCD"; }
        public string LastUpdate { get => "ABCD"; }
        public string Profit { get => "ABCD"; }
        public string TrendPercentage { get => "ABCD"; }
    }*/

    public interface IStock : ISeries<FinancialPoint>
    {
        public string ShortName { get; set; }
        public string LongName { get; set; }
    }
}