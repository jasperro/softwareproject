using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using LiveChartsCore.SkiaSharpView;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SoftwareProject.Types;
using SoftwareProject.Views;
using static SoftwareProject.Globals;
using static SoftwareProject.ViewModels.MainWindowViewModel;

namespace SoftwareProject.ViewModels
{
    public class HomePageViewModel : ViewModelBase
    {
        private string _newstockname;

        /// <summary>Stocks that are followed</summary>
        public ObservableCollection<Stock> Stocks { get; }

        /// <summary>Stocks that are visible in the chart</summary>
        public ObservableCollection<Stock> Series { get; set; }

        public HomePageViewModel()
        {
            Stocks = new ObservableCollection<Stock> { new() };

            Series = new ObservableCollection<Stock>();
        }

        public string NewStockName
        {
            get => _newstockname;
            set => this.RaiseAndSetIfChanged(ref _newstockname, value);
        }

        public Axis[] XAxes { get; set; } =
        {
            new()
            {
                LabelsRotation = 15,
                Labeler = value => new DateTime((long)value).ToString("yyyy MMM dd"),
                UnitWidth = TimeSpan.FromDays(1).Ticks
            }
        };

        public IObservable<string> CurrentDateString =>
            Timekeeping.WhenAny(x => x.CurrentTime, _ => Timekeeping.CurrentTime.ToLongDateString());

        [Reactive] public bool TimerRunning { get; set; } = Timekeeping.Timer.Enabled;

        public void ToggleTimer()
        {
            Timekeeping.Timer.Enabled = !Timekeeping.Timer.Enabled;
            TimerRunning = Timekeeping.Timer.Enabled;
        }

        public void ApplyAlgorithmOpen(string shortName)
        {
            // This is a bad way of doing things, but it seems to work.
            // Can be improved by using ReactiveUI, but that's quite convoluted.
            var algorithmapplicator = new AlgorithmApplicator
            {
                DataContext =
                    new AlgorithmApplicatorViewModel("AAPL", new CalendarDateRange(DateTime.Now, DateTime.Now))
            };
            Task windowtask;
            if (Avalonia.Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                windowtask = algorithmapplicator.ShowDialog(desktop.MainWindow);
            }
        }
    }
}