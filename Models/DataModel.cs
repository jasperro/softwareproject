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
            public StockPoint(DateTime date, double high, double open, double close, double low, int volume) : base(date, high, open, close, low)
            {
                Volume = volume;
            }
            public int Volume { get; set; }
        }

        public class Stock : IStock
        {
            public string ShortName { get; set; } = "AAPL";
            public string FullName { get; set; } = "Apple";
            public ISeries<StockPoint> StockPoints { get; set; } = new CandlesticksSeries<StockPoint> {};
        }
        public interface IStock
        {
           public string ShortName { get; set; } 
           public string FullName { get; set; }

           public ISeries<StockPoint> StockPoints
           {
               get;
               set;
           }
        }
    }
}