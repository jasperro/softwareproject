using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
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
        /// <summary>Stocks that are followed</summary>
        public Collection<Stock> Stocks { get; }

        /// <summary>Stocks that are visible in the chart</summary>
        public Collection<Stock> Series { get; set; }

        public HomePageViewModel()
        {
            Stocks = new Collection<Stock> { new() };

            Series = new Collection<Stock>();
        }

        [Reactive] public string NewStockName { get; set; } = "";

        public IObservable<string> CurrentDateString =>
            Timekeeping.WhenAny(x => x.CurrentTime, _ => Timekeeping.CurrentTime.Date.ToLongDateString());

        [Reactive] public bool TimerRunning { get; set; } = Timekeeping.Timer.Enabled;

        [Reactive]
        public DateTimeOffset? SelectedDate
        {
            get;
            set;
        }

        [Reactive]
        public bool FollowTicker
        {
            get;
            set;
        }

        public void ChangeDateToSelected()
        {
            if (SelectedDate != null) Timekeeping.CurrentTime = SelectedDate.Value;
        }

        public void AddStock()
        {
            Stock newStock;

            try
            {
                newStock = GetStock(NewStockName);
            }
            catch
            {
                newStock = new Stock(NewStockName);
            }

            Stocks.Add(newStock);
            Series.Add(Stocks.Last());
        }

        public void ToggleTimer()
        {
            Timekeeping.Timer.Enabled = !Timekeeping.Timer.Enabled;
            TimerRunning = Timekeeping.Timer.Enabled;
        }

        public void ApplyAlgorithmOpen(string shortName)
        {
            // This is a bad way of doing things, but it seems to work.
            // Can be improved by using ReactiveUI, but that's quite convoluted.
            var algorithmApplicator = new AlgorithmApplicator
            {
                DataContext =
                    new AlgorithmApplicatorViewModel("AAPL", new CalendarDateRange(DateTime.Now, DateTime.Now))
            }; 
            if (Avalonia.Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                algorithmApplicator.ShowDialog(desktop.MainWindow);
            }
        }
    }
}