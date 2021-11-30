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
            public StockPoint(DateTime date, double high, double open, double close, double low, int volume = 0) : base(date, high, open, close, low)
            {
                Volume = volume;
            }
            public int Volume { get; set; }
        }

        public class Stock : IStock
        {
            private readonly ObservableCollection<FinancialPoint> _observableValues;
            public string ShortName { get; set; } = "AAPL";
            public string FullName { get; set; } = "Apple";

            public Stock()
            {
                _observableValues = new ObservableCollection<FinancialPoint>
                {
                    new StockPoint(new DateTime(2021, 1, 1), 523, 500, 450, 400),
                    new StockPoint(new DateTime(2021, 1, 2), 500, 450, 425, 400),
                    new StockPoint(new DateTime(2021, 1, 3), 490, 425, 400, 380),
                    new StockPoint(new DateTime(2021, 1, 4), 420, 400, 420, 380),
                    new StockPoint(new DateTime(2021, 1, 5), 520, 420, 490, 400),
                    new StockPoint(new DateTime(2021, 1, 6), 580, 490, 560, 440),
                    new StockPoint(new DateTime(2021, 1, 7), 570, 560, 350, 340),
                    new StockPoint(new DateTime(2021, 1, 8), 380, 350, 380, 330),
                    new StockPoint(new DateTime(2021, 1, 9), 440, 380, 420, 350),
                    new StockPoint(new DateTime(2021, 1, 10), 490, 420, 460, 400),
                    new StockPoint(new DateTime(2021, 1, 11), 520, 460, 510, 460),
                    new StockPoint(new DateTime(2021, 1, 12), 580, 510, 560, 500),
                    new StockPoint(new DateTime(2021, 1, 13), 600, 560, 540, 510),
                    new StockPoint(new DateTime(2021, 1, 14), 580, 540, 520, 500),
                    new StockPoint(new DateTime(2021, 1, 15), 580, 520, 560, 520),
                    new StockPoint(new DateTime(2021, 1, 16), 590, 560, 580, 520),
                    new StockPoint(new DateTime(2021, 1, 17), 650, 580, 630, 550),
                    new StockPoint(new DateTime(2021, 1, 18), 680, 630, 650, 600),
                    new StockPoint(new DateTime(2021, 1, 19), 670, 650, 600, 570),
                    new StockPoint(new DateTime(2021, 1, 20), 640, 600, 610, 560),
                    new StockPoint(new DateTime(2021, 1, 21), 630, 610, 630, 590),
                };
                
                StockPoints = new CandlesticksSeries<FinancialPoint>
                                          {
                                              Values = _observableValues
                                          };
            }

            public ISeries<FinancialPoint> StockPoints { get; set; }
        }
        public interface IStock
        {
           public string ShortName { get; set; } 
           public string FullName { get; set; }

           public ISeries<FinancialPoint> StockPoints
           {
               get;
               set;
           }
        }
    }
}