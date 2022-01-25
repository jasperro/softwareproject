using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SkiaSharp;
using SoftwareProject.Types;
using SoftwareProject.Views;
using static SoftwareProject.Globals;
using static SoftwareProject.ViewModels.MainWindowViewModel;

namespace SoftwareProject.ViewModels
{
    public class HomePageViewModel : ViewModelBase
    {
        /// <summary>
        /// The main stock that is viewed, is DailyStock by default, but can be changed to a IntradayStock
        /// </summary>
        [Reactive]
        public Stock? MainStock { get; set; }

        [Reactive] public Stock? DailyStock { get; set; }

        /// <summary>
        /// Extra stocks that are viewed in the graph, for example in the case of a stock comparison.
        /// </summary>
        public ObservableCollection<Stock> ExtraStocks { get; }

        /// <summary>Lines/points that are visible in the chart</summary>
        public ObservableCollection<ISeries> Series { get; set; }

        public HomePageViewModel()
        {
            ExtraStocks = new ObservableCollection<Stock> { new() };

            Series = new ObservableCollection<ISeries>();

            Timekeeping.ObservableTimer.Subscribe(_ =>
            {
                if (FollowTicker) ResetGraphPosition();
                if (!DayByDayMode)
                {
                    try
                    {
                        if (MainStock != null)
                            SelectedViewDate = MainStock.Values == null
                                ? Timekeeping.CurrentTime
                                : MainStock.Values!.Last().Date;
                    }
                    catch(Exception e) {}
                }
            });

            this.ObservableForProperty(x => x.SelectedViewDate).Subscribe(_ =>
            {
                if (DayByDayMode)
                    (XAxes[0].MinLimit, XAxes[0].MaxLimit) = (SelectedViewDate.Date.AddHours(-2).Ticks,
                        SelectedViewDate.Date.AddDays(1).AddHours(2).Ticks);
            });

            this.ObservableForProperty(x => x.ShowCandleSticks).Subscribe(_ =>
            {
                if (Series.ElementAtOrDefault(0) != null) Series[0].IsVisible = ShowCandleSticks;
            });
            this.ObservableForProperty(x => x.ShowTrendLine).Subscribe(_ =>
            {
                if (Series.ElementAtOrDefault(1) != null)
                {
                    Series[1].IsVisible = ShowTrendLine;
                    ResetGraphPosition();
                }
            });
            this.ObservableForProperty(x => x.ShowLineGraph).Subscribe(_ =>
            {
                if (Series.ElementAtOrDefault(2) != null) Series[2].IsVisible = ShowLineGraph;
            });
        }

        public void ResetGraphPosition()
        {
            (XAxes[0].MinLimit, XAxes[0].MaxLimit) = (null, null);
            DayByDayMode = false;
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
            Timekeeping.WhenAny(x => x.CurrentTime,
                _ => Timekeeping.CurrentTime.ToLocalTime().ToString(DateTimeFormatInfo.CurrentInfo));

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


        [Reactive] public bool ShowCandleSticks { get; set; } = true;
        [Reactive] public bool ShowTrendLine { get; set; }
        [Reactive] public bool ShowLineGraph { get; set; }
        [Reactive] public bool ShowSettings { get; set; } = true;

        private SKColor _downcolor1 = SKColors.OrangeRed;
        private SKColor _downcolor2 = new(159, 18, 57);
        private SKColor _upcolor1 = new(16, 185, 129);
        private SKColor _upcolor2 = new(22, 163, 74);

        public void ViewStock(Stock? stock = null, bool daily = true)
        {
            Series.Clear();
            MainStock = stock ?? GetStock(NewStockName);
            if (MainStock == null) return;
            // Candle graph
            MainStock.IsVisible = ShowCandleSticks;
            MainStock.MaxBarWidth = 4;
            MainStock.UpFill = new LinearGradientPaint(_upcolor1, _upcolor2);
            MainStock.DownFill = new LinearGradientPaint(_downcolor1, _downcolor2);
            MainStock.UpStroke = new LinearGradientPaint(_upcolor1, _upcolor2)
                { StrokeThickness = 2, StrokeCap = SKStrokeCap.Round };
            MainStock.DownStroke = new LinearGradientPaint(_downcolor1, _downcolor2)
                { StrokeThickness = 2, StrokeCap = SKStrokeCap.Round };
            Series.Add(MainStock);
            // Trend line
            Series.Add(new LineSeries<ObservablePoint>
            {
                IsVisible = ShowTrendLine,
                GeometrySize = 1,
                Name = $"{NewStockName} Trend",
                Stroke = new SolidColorPaint(SKColors.Aquamarine, 5),
                GeometryStroke = new SolidColorPaint(SKColors.Empty),
                GeometryFill = new SolidColorPaint(SKColors.Empty),
                Fill = new SolidColorPaint(SKColor.Empty)
            });
            // Line graph
            Series.Add(new LineSeries<ObservablePoint>
            {
                IsVisible = ShowLineGraph,
                Name = $"{NewStockName} Line",
                LineSmoothness = 200,
                GeometrySize = 1,
                Stroke = new LinearGradientPaint(SKColors.Orange, SKColors.OrangeRed)
                    { StrokeThickness = 2, StrokeCap = SKStrokeCap.Round },
                GeometryStroke = new SolidColorPaint(SKColors.Empty),
                GeometryFill = new SolidColorPaint(SKColors.Empty),
                Fill = new SolidColorPaint(SKColors.Orange.WithAlpha(20)),
            });
            MainStock.ObservableForProperty(x => x.Values).Subscribe(_ =>
            {
                if (MainStock.Values == null) return;
                if (MainStock.Values!.Any() && Series.Any())
                {
                    Series[1].Values = new[]
                    {
                        new ObservablePoint
                        {
                            X = (double)MainStock.Values!.First().Date.Ticks, Y = MainStock.Values!.First().Open
                        },
                        new ObservablePoint
                            { X = (double)MainStock.Values!.Last().Date.Ticks, Y = MainStock.Values!.Last().Close }
                    };
                    Series[2].Values = MainStock.Values!.Select(x => new ObservablePoint
                        { X = (double)x.Date.Ticks, Y = (x.Open + x.Close) / 2 }).ToArray();
                }
            });

            if (!daily) return;
            // Cache if daily stock
            DailyStock = MainStock;
            // Reset the view to make the whole graph visible, and start tracking the ticker
            ResetGraphPosition();
            FollowTicker = true;
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


        public void ViewPreviousDay()
        {
            SelectedViewDate = SelectedViewDate.AddDays(-1);
            DayByDayMode = true;
        }

        public void ViewNextDay()
        {
            SelectedViewDate = SelectedViewDate.AddDays(1);
            DayByDayMode = true;
        }

        [Reactive] public DateTimeOffset SelectedViewDate { get; set; } = Timekeeping.CurrentTime;

        private bool _dayByDayMode;

        public bool DayByDayMode
        {
            get => _dayByDayMode;
            set
            {
                if (value)
                {
                    // Reset series for the case where there is no data
                    Series.Clear();
                    // Stop following the ticker when day by day mode is started
                    FollowTicker = false;
                    // Load the intraday data
                    ViewStock(CurrentDatabase.GetStockFromDb(MainStock.ShortName, SelectedViewDate), false);
                    XAxes[0].UnitWidth = TimeSpan.FromMinutes(1).Ticks;
                    XAxes[0].Labeler = value => new DateTime((long)value).ToString("HH:mm");
                    XAxes[0].LabelsRotation = 90;
                    XAxes[0].MinStep = TimeSpan.FromMinutes(15).Ticks;
                    XAxes[0].ForceStepToMin = true;
                }
                else
                {
                    XAxes[0].UnitWidth = TimeSpan.FromDays(1).Ticks;
                    XAxes[0].Labeler = value => new DateTime((long)value).ToString("yyyy MMM dd");
                    XAxes[0].LabelsRotation = 15;
                    XAxes[0].MinStep = Double.NaN;
                    XAxes[0].ForceStepToMin = false;
                }

                this.RaiseAndSetIfChanged(ref _dayByDayMode, value);
            }
        }

        public IObservable<ZoomAndPanMode> ChartZoomMode => this.WhenAny(x => x.DayByDayMode,
            s => s.Value ? ZoomAndPanMode.None : ZoomAndPanMode.X);

        public void PreviewStock()
        {
            ViewStock();
        }

        /// <summary>
        /// Reset chart back to the default daily view
        /// </summary>
        public void ResetChartMode()
        {
            PreviewStock();
            FollowTicker = true;
            SelectedViewDate = Timekeeping.CurrentTime;
        }
    }
}