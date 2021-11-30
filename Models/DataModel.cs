using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using ReactiveUI;

namespace SoftwareProject.Models
{
    public class DataModel : ReactiveObject
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
            public string ShortName { get; set; } = "AAPL";
            public string FullName { get; set; } = "Apple";

            public Stock(ObservableCollection<FinancialPoint>? defaultData = null)
            {
                // Set default values if stock has no data yet.
                _observableValues = defaultData ?? new ObservableCollection<FinancialPoint>
                {
                };

                Values = _observableValues;
            }
        }

        public interface IStock : ISeries<FinancialPoint>
        {
            public string ShortName { get; set; }
            public string FullName { get; set; }
        }
    }
}
