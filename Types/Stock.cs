using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.Kernel;
using LiveChartsCore.SkiaSharpView;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SoftwareProject.Algorithms;
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
        [Reactive] public ObservableCollection<FinancialPoint> AllValues { get; set; }

        public string LongName { get; set; } = "Abcd efghi";

        public string ShortName
        {
            get => Name!;
            set => Name = value;
        }

        public DateTime LastUpdate => Values != null ? Values.Last().Date : DateTime.Now;

        public double TrendPercentage => 0;

        public Stock(string shortName = "ABCD", ObservableCollection<FinancialPoint>? defaultData = null)
        {
            // Set default values if stock has no data yet.
            AllValues = defaultData ?? new ObservableCollection<FinancialPoint>();

            ShortName = shortName;

            MainWindowViewModel.Timekeeping.ObservableTimer.Subscribe(_ =>
            {
                Values = AllValues.Where(financialPoint =>
                    financialPoint.Date.CompareTo(MainWindowViewModel.Timekeeping.CurrentTime.DateTime) < 0);
            });
        }

        /// <summary>Update all stocks data to match current application time</summary>
        public void UpdateToTime(DateTimeOffset currentTime)
        {
        }
    }


    public interface IStock : ISeries<FinancialPoint>
    {
        public string ShortName { get; set; }
        public string LongName { get; set; }

        public DateTime LastUpdate { get; }
    }
}