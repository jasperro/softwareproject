using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.Kernel;
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
        /// <summary>
        /// Stocks that are viewed in the graph, this should only be one stock in most cases,
        /// except in the case of a stock comparison.
        /// </summary>
        public ObservableCollection<Stock> Stocks { get; }

        /// <summary>Stocks that are visible in the chart</summary>
        public ObservableCollection<ISeries> Series { get; set; }

        public HomePageViewModel()
        {
            Stocks = new ObservableCollection<Stock> { new() };

            Series = new ObservableCollection<ISeries>();

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

        [Reactive] public DateTimeOffset? SelectedDate { get; set; }

        [Reactive] public bool FollowTicker { get; set; }


        [Reactive] public string NewTickInterval { get; set; } = Timekeeping.TickInterval.ToString();

        [Reactive] public string NewTimeStep1Second { get; set; } = Timekeeping.TimeStep1Second.ToString();

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

        public void ViewStock()
        {
            Stocks.Clear();
            Series.Clear();
            Stocks.Add(GetStock(NewStockName));
            Series.Add(Stocks[0]);
            Series.Add(new LineSeries<ObservablePoint> { Name = $"{NewStockName} Trend" });
            Stocks[0].ObservableForProperty(x => x.Values).Subscribe(_ =>
            {
                if (Stocks[0].Values!.Any())
                {
                    Series[1].Values = new[]
                    {
                        new ObservablePoint
                        {
                            X = (double)Stocks[0].Values!.First().Date.Ticks, Y = Stocks[0].Values!.First().Open
                        },
                        new ObservablePoint
                            { X = (double)Stocks[0].Values!.Last().Date.Ticks, Y = Stocks[0].Values!.Last().Close }
                    };
                }
            });
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