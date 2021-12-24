using System;
using Avalonia.Controls;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace SoftwareProject.ViewModels
{
    public class AlgorithmApplicatorViewModel : ViewModelBase
    {
        public string ShortName { get; } = "";

        public CalendarDateRange? DateRange { get; }

        [Reactive] public ComboBoxItem? SelectedAlgorithmComboBoxItem { get; set; }

        public ComboBoxItem[] AlgorithmComboBoxItems { get; } =
            { new() { Content = "item 1" }, new() { Content = "item 2" } };

        public IObservable<string> SelectedAlgorithmName =>
            this.WhenAny(x => x.SelectedAlgorithmComboBoxItem, s => s.Value?.Content.ToString() ?? "No selection!");

        public AlgorithmApplicatorViewModel()
        {
        }

        public AlgorithmApplicatorViewModel(string shortName, CalendarDateRange dateRange)
        {
            ShortName = shortName;
            DateRange = dateRange;
        }
    }
}