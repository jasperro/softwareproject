using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SoftwareProject.Algorithms;
using SoftwareProject.Types;
using DesktopNotifications;

namespace SoftwareProject.ViewModels
{
    public class AlgorithmApplicatorViewModel : ViewModelBase
    {
        private readonly INotificationManager _notificationManager;
        public static IEnumerable<IAlgorithm> AlgorithmList => Algorithms.AlgorithmHelpers.AlgorithmList;
        public string ShortName { get; } = "";

        public CalendarDateRange? DateRange { get; }

        [Reactive] public IAlgorithm SelectedAlgorithmListItem { get; set; } = AlgorithmList.First();

        private IStock CurrentStock { get; set; }


        /// <summary>Stocks that are visible in the test chart</summary>
        public ObservableCollection<IStock> Series { get; set; } = new();

        public Axis[] XAxes { get; set; } =
        {
            new()
            {
                LabelsRotation = 15,
                Labeler = value => new DateTime((long)value).ToString("yyyy MMM dd"),
                UnitWidth = TimeSpan.FromDays(1).Ticks
            }
        };

        public void ApplyAlgorithm()
        {
            _notificationManager.ShowNotification(new Notification
                { Title = "Algorithm Application", Body = $"Applying {SelectedAlgorithmListItem.AlgorithmId}" });
            Series.Clear();
            Series.Insert(0, CurrentStock);
            Series.Insert(1, SelectedAlgorithmListItem.Apply(CurrentStock));
        }

        public AlgorithmApplicatorViewModel(string shortName, CalendarDateRange dateRange)
        {
            ShortName = shortName;
            DateRange = dateRange;
            CurrentStock = Globals.CurrentDatabase.GetStockFromDb(shortName);
            _notificationManager = AvaloniaLocator.Current.GetService<INotificationManager>(); 
        }
    }
}