using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia.Collections;
using Avalonia.Controls;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SoftwareProject.Algorithms;
using SoftwareProject.Types;
using static SoftwareProject.Globals;

namespace SoftwareProject.ViewModels
{
    public class AlgorithmApplicatorViewModel : ViewModelBase
    {
        public static IEnumerable<IStockAlgorithm> AlgorithmList => Algorithms.AlgorithmHelpers.StockAlgorithmList;
        public string ShortName { get; } = "";

        public CalendarDateRange? DateRange { get; }

        [Reactive] public IStockAlgorithm SelectedStockAlgorithmListItem { get; set; } = AlgorithmList.First();

        private IStock CurrentStock { get; set; }


        /// <summary>Stocks that are visible in the test chart</summary>
        public ObservableCollection<IStock> Series { get; set; } = new();
        public Axis[] XAxes { get; set; } = {
            new()
            {
                LabelsRotation = 15,
                Labeler = value => new DateTime((long) value).ToString("yyyy MMM dd"),
                UnitWidth = TimeSpan.FromDays(1).Ticks
            }
        };

        public void ApplyAlgorithm()
        {
            Console.WriteLine($"Applying {SelectedStockAlgorithmListItem.AlgorithmId}");
            Series.Clear();
            Series.Insert(0, CurrentStock);
            Series.Insert(1, SelectedStockAlgorithmListItem.Apply(CurrentStock));
        }

        public AlgorithmApplicatorViewModel(string shortName, CalendarDateRange dateRange)
        {
            ShortName = shortName;
            DateRange = dateRange;
            CurrentStock = GetStock(shortName);
        }
    }
}