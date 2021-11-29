using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ReactiveUI;

namespace SoftwareProject.Models
{
    public class DataModel : ReactiveObject
    {
        public class StockPoint
        {
            public StockPoint(DateTime date, decimal open, decimal close, decimal high, decimal low, int volume)
            {
                Date = date;
                Open = open;
                Close = close;
                High = high;
                Low = low;
                Volume = volume;
            }

            public DateTime Date { get; set; }
            public decimal Open { get; set; }
            public decimal Close { get; set; }
            public decimal High { get; set; }
            public decimal Low { get; set; }
            public int Volume { get; set; }
        }

        public class Stock : IStock
        {
            public string ShortName { get; set; } = "AAPL";
            public string FullName { get; set; } = "Apple";
            public ICollection<StockPoint> StockPoints { get; set; } = new ObservableCollection<StockPoint> {};
        }
        public interface IStock
        {
           public string ShortName { get; set; } 
           public string FullName { get; set; }

           public ICollection<StockPoint> StockPoints
           {
               get;
               set;
           }
        }
    }
}