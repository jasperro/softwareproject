using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia.Collections;
using Avalonia.Controls;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace SoftwareProject.ViewModels
{
    public class AlgorithmApplicatorViewModel : ViewModelBase
    {
        public static IEnumerable<KeyValuePair<string, IAlgorithm>> AlgorithmList => Algorithms.AlgorithmList;
        public string ShortName { get; } = "";

        public CalendarDateRange? DateRange { get; }

        [Reactive] public KeyValuePair<string, IAlgorithm> SelectedAlgorithmListItem { get; set; }

        public void ApplyAlgorithm()
        {
            var selectedAlgorithm = SelectedAlgorithmListItem.Value;
            Console.WriteLine($"Applying {selectedAlgorithm.AlgorithmId}");
            selectedAlgorithm.Apply(ShortName);
        }

        public AlgorithmApplicatorViewModel(string shortName, CalendarDateRange dateRange)
        {
            ShortName = shortName;
            DateRange = dateRange;
        }
    }
}