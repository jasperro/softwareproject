using System;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SkiaSharp;
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
                if (FollowTicker) ResetGraphPosition();
            });

            this.ObservableForProperty(x => x.ShowCandleSticks).Subscribe(_ =>
            {
                if (Series.ElementAtOrDefault(0) != null) Series[0].IsVisible = ShowCandleSticks;
            });
            this.ObservableForProperty(x => x.ShowTrendLine).Subscribe(_ =>
            {
                if (Series.ElementAtOrDefault(1) != null) Series[1].IsVisible = ShowTrendLine;
            });
            this.ObservableForProperty(x => x.ShowLineGraph).Subscribe(_ =>
            {
                if (Series.ElementAtOrDefault(2) != null) Series[2].IsVisible = ShowLineGraph;
            });
        }

        public void ResetGraphPosition()
        {
            (XAxes[0].MinLimit, XAxes[0].MaxLimit) = (null, null);
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

        private bool _followTicker;

        public bool FollowTicker
        {
            get => _followTicker;
            set
            {
                // Instantly reset the graph if the user starts following the ticker
                if (value) ResetGraphPosition();
                this.RaiseAndSetIfChanged(ref _followTicker, value);
            }
        }

        [Reactive] public string NewTickInterval { get; set; } = Timekeeping.TickInterval.ToString();

        [Reactive] public string NewTimeStep1Second { get; set; } = Timekeeping.TimeStep1Second.ToString();

        [Reactive] public bool ShowCandleSticks { get; set; } = true;
        [Reactive] public bool ShowTrendLine { get; set; }
        [Reactive] public bool ShowLineGraph { get; set; }

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

        public void ViewStock(Stock? stock = null)
        {
            Stocks.Clear();
            Series.Clear();
            Stocks.Add(stock ?? GetStock(NewStockName));
            // Candle graph
            Stocks[0].IsVisible = ShowCandleSticks;
            Stocks[0].MaxBarWidth = 4;
            Series.Add(Stocks[0]);
            // Trend line
            Series.Add(new LineSeries<ObservablePoint>
            {
                IsVisible = ShowTrendLine,
                GeometrySize = 0,
                Name = $"{NewStockName} Trend",
                Stroke = new SolidColorPaint(SKColors.Aquamarine, 5),
                GeometryStroke = new SolidColorPaint(SKColors.Empty),
                Fill = new SolidColorPaint(SKColor.Empty)
            });
            // Line graph
            Series.Add(new LineSeries<ObservablePoint>
            {
                IsVisible = ShowLineGraph,
                Name = $"{NewStockName} Line",
                LineSmoothness = 200,
                GeometrySize = 0,
                Stroke = new SolidColorPaint(SKColors.Orange, 2),
                GeometryStroke = new SolidColorPaint(SKColors.Empty),
                Fill = new SolidColorPaint(SKColors.Orange.WithAlpha(20))
            });
            Stocks[0].ObservableForProperty(x => x.Values).Subscribe(_ =>
            {
                if (Stocks[0].Values == null) return;
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
                    Series[2].Values = Stocks[0].Values!.Select(x => new ObservablePoint
                        { X = (double)x.Date.Ticks, Y = (x.Open + x.Close) / 2 }).ToArray();
                }
            });
            // Reset the view to make the whole graph visible, and start tracking the ticker
            ResetGraphPosition();
            FollowTicker = true;
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