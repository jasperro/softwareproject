using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia.Collections;
using Avalonia.Controls;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SoftwareProject.Algorithms;

namespace SoftwareProject.ViewModels
{
    public class AlgorithmApplicatorViewModel : ViewModelBase
    {
        public static IEnumerable<IAlgorithm> AlgorithmList => Algorithms.AlgorithmHelpers.AlgorithmList;
        public string ShortName { get; } = "";

        public CalendarDateRange? DateRange { get; }

        [Reactive] public IAlgorithm SelectedAlgorithmListItem { get; set; }

        public void ApplyAlgorithm()
        {
            Console.WriteLine($"Applying {SelectedAlgorithmListItem.AlgorithmId}");
            SelectedAlgorithmListItem.Apply(ShortName);
        }

        public AlgorithmApplicatorViewModel(string shortName, CalendarDateRange dateRange)
        {
            ShortName = shortName;
            DateRange = dateRange;
        }
    }
}