using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Avalonia.Controls;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SkiaSharp;
using SoftwareProject.Models;
using SoftwareProject.Pages;

namespace SoftwareProject.ViewModels
{
    public class SettingsPageViewModel : ViewModelBase
    {
        private UserModel _userModel => MainWindowViewModel.User;

        private TimekeepingModel _timeKeeping => MainWindowViewModel.Timekeeping;

        public string Username
        {
            get => _userModel.Username;
            set => _userModel.Username = value;
        }

        public IObservable<string> Greeting => _userModel.WhenAny(x => x.Username, s => "Uw naam is: " + s.Value);

        public string Ticker { set; get; } = "";

        [Reactive] public string Interval { get; set; } = ApiIntervalList[0];

        [Reactive] public DateTimeOffset ImportDatum { get; set; } = DateTimeOffset.Now;

        public static string[] ApiIntervalList => new[]
        {
            "daily", "1min", "5min", "15min", "30min", "60min"
        };

        public void ApplyUserSettings()
        {
            // Apply user settings, possibly not needed
        }

        public void ApiImportButton()
        {
            ApiModel.DataImport(Ticker, ImportDatum, Interval);
            Globals.CurrentDatabase.ImportTestData();
        }
    }
}