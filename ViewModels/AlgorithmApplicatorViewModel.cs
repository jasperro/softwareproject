using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia.Collections;
using Avalonia.Controls;
using OxyPlot.Axes;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SoftwareProject.Algorithms;
using SoftwareProject.Types;
using static SoftwareProject.Globals;

namespace SoftwareProject.ViewModels
{
    public class AlgorithmApplicatorViewModel : ViewModelBase
    {
        public static IEnumerable<IAlgorithm> AlgorithmList => Algorithms.AlgorithmHelpers.AlgorithmList;
        public string ShortName { get; } = "";

        public CalendarDateRange? DateRange { get; }

        [Reactive] public IAlgorithm SelectedAlgorithmListItem { get; set; } = AlgorithmList.First();

        private Stock CurrentStock { get; set; }


        /// <summary>Stocks that are visible in the test chart</summary>
        public Collection<Stock> Series { get; set; } = new();

        public void ApplyAlgorithm()
        {
            Console.WriteLine($"Applying {SelectedAlgorithmListItem.AlgorithmId}");
            Series.Clear();
            Series.Insert(0, CurrentStock);
            Series.Insert(1, SelectedAlgorithmListItem.Apply(CurrentStock));
        }

        public AlgorithmApplicatorViewModel(string shortName, CalendarDateRange dateRange)
        {
            ShortName = shortName;
            DateRange = dateRange;
            CurrentStock = GetStock(shortName);
        }
    }
}