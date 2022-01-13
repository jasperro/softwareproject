using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using LiveChartsCore.SkiaSharpView;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SoftwareProject.Types;
using SoftwareProject.Views;
using Splat;
using static SoftwareProject.Globals;
using static SoftwareProject.ViewModels.MainWindowViewModel;

namespace SoftwareProject.ViewModels
{
    public class HomePageViewModel : ViewModelBase
    {
        /// <summary>Stocks that are followed</summary>
        public ObservableCollection<Stock> Stocks { get; }

        /// <summary>Stocks that are visible in the chart</summary>
        public ObservableCollection<Stock> Series { get; set; }

        public HomePageViewModel()
        {
            Stocks = new ObservableCollection<Stock> { new() };

            Series = new ObservableCollection<Stock>();

            Timekeeping.ObservableTimer.Subscribe(_ =>
            {
                if (FollowTicker) (XAxes[0].MinLimit, XAxes[0].MaxLimit) = (null, null);
            });
        }

        [Reactive] public string NewStockName { get; set; } = "";

        public Axis[] XAxes { get; set; } =
        {
            new()
            {
                LabelsRotation = 15,
                Labeler = value => new DateTime((long)value).ToString("yyyy MMM dd"),
                UnitWidth = TimeSpan.FromDays(1).Ticks,
                MinLimit = null,
                MaxLimit = null
            }
        };

        public IObservable<string> CurrentDateString =>
            Timekeeping.WhenAny(x => x.CurrentTime, _ => Timekeeping.CurrentTime.ToString());

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
        
        

        [Reactive]
        public string NewTickInterval
        {
            get;
            set;
        } = Timekeeping.TickInterval.ToString();
        
        [Reactive]
        public string NewTimeStep1Second
        {
            get;
            set;
        } = Timekeeping.TimeStep1Second.ToString();

        public void ChangeDateToSelected()
        {
            try
            {
                if (SelectedDate != null) Timekeeping.CurrentTime = SelectedDate.Value;
                Timekeeping.TickInterval = TimeSpan.Parse(NewTickInterval);
                Timekeeping.TimeStep1Second = TimeSpan.Parse(NewTimeStep1Second);
            }
            catch
            {
                Logs.Write("Date invalid", LogLevel.Debug);
            }
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