using System;
using System.Collections.ObjectModel;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using ReactiveUI;

namespace SoftwareProject.Models
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

    public interface IStock : ISeries<FinancialPoint>
    {
        public string ShortName { get; set; }
        public string LongName { get; set; }
    }

    public class DataModel : ReactiveObject
    {
    }
}