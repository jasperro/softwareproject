using System;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SoftwareProject.Models;
using Splat;
using static SoftwareProject.ViewModels.MainWindowViewModel;
using static SoftwareProject.Globals;

namespace SoftwareProject.ViewModels
{
    public class SettingsPageViewModel : ViewModelBase
    {
        private UserModel _userModel => User;

        public string Username
        {
            get => _userModel.Username;
            set => _userModel.Username = value;
        }

        public string Ticker { set; get; } = "";

        [Reactive] public string Interval { get; set; } = ApiIntervalList[0];

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
            ApiModel.DataImport(Ticker, Interval, DataImportType, IntradayMonthAmount, IntradayStartMonthsAgo);
            CurrentDatabase.ImportTestData();
        }

        [Reactive] public ApiModel.ImportType DataImportType { get; set; } = ApiModel.ImportType.Stock;

        // Time settings
        public IObservable<string> CurrentDateString =>
            Timekeeping.WhenAny(x => x.CurrentTime, _ => Timekeeping.CurrentTime.ToString());

        [Reactive] public DateTimeOffset? SelectedDate { get; set; }
        [Reactive] public string NewTickInterval { get; set; } = Timekeeping.TickInterval.ToString();
        [Reactive] public bool TimerRunning { get; set; } = Timekeeping.Timer.Enabled;
        [Reactive] public string NewTimeStep1Second { get; set; } = Timekeeping.TimeStep1Second.ToString();
        [Reactive] public int IntradayMonthAmount { get; set; }
        [Reactive] public int IntradayStartMonthsAgo { get; set; }

        public IObservable<bool> DailySelected => this.WhenAny(x => x.Interval, x => x.DataImportType,
            (i, t) => i.Value == "daily" || t.Value == ApiModel.ImportType.Crypto);

        [Reactive] public bool DownloadRange { get; set; } = true;

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

        public void ToggleTimer()
        {
            Timekeeping.Timer.Enabled = !Timekeeping.Timer.Enabled;
            TimerRunning = Timekeeping.Timer.Enabled;
        }
    }
}