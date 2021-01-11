using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using OxyPlot.Axes;
using OxyPlot.Series;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SoftwareProject.Algorithms;
using SoftwareProject.ViewModels;

namespace SoftwareProject.Types
{
    public class StockPoint : HighLowItem
    {
        public StockPoint(DateTime date, double high, double open, double close, double low, int volume = 0) : base(
            DateTimeAxis.ToDouble(date), high, open, close, low)
        {
            Volume = volume;
        }

        public int Volume { get; set; }
        public DateTime Date => DateTimeAxis.ToDateTime(X);
    }

    public class StockStickSeries : CandleStickSeries
    {
        protected new IEnumerable<StockPoint> Items
        {
            get;
            private set;
        }
        
        
        protected new IEnumerable<StockPoint> ItemsSource
        {
            set => Items = value;
        }

    }

    public class Stock : StockStickSeries
    {
        [Reactive] public Collection<StockPoint> AllValues { get; set; }

        public string LongName { get; set; } = "Abcd efghi";

        public string ShortName
        {
            get => Title!;
            set => Title = value;
        }

        public DateTime LastUpdate => Items != null ? Items.Last().Date : DateTime.Now;

        public double TrendPercentage => 0;

        public Stock(string shortName = "ABCD", Collection<StockPoint>? defaultData = null)
        {
            // Set default values if stock has no data yet.
            AllValues = defaultData ?? new Collection<StockPoint>();

            ShortName = shortName;

            MainWindowViewModel.Timekeeping.ObservableTimer.Subscribe(_ =>
            {
                var valuesBeforeDate = AllValues.Where(financialPoint =>
                    financialPoint.Date.CompareTo(MainWindowViewModel.Timekeeping.CurrentTime.DateTime) < 0);
                if (valuesBeforeDate.Any())
                {
                    ItemsSource = valuesBeforeDate;
                }
            });
        }

        /// <summary>Update all stocks data to match current application time</summary>
        public void UpdateToTime(DateTimeOffset currentTime)
        {
        }
    }


    public interface IStock
    {
        public string ShortName { get; set; }
        public string LongName { get; set; }

        public DateTime LastUpdate { get; }
    }
}