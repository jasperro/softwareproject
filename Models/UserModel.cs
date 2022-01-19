using System;
using System.Collections.Generic;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SoftwareProject.Types;
using static SoftwareProject.Models.ApiModel;
using SoftwareProject.ViewModels;

namespace SoftwareProject.Models
{
    /// <summary>
    /// Stores all the information that only the current user has access to.
    /// <example>Username, Investments, User settings</example>
    /// </summary>
    public class UserModel : ReactiveObject
    {
        public int UserId { get; }

        public UserModel(int userId = 0)
        {
            UserId = userId;
            UserInvestmentPortfolio =
                Globals.CurrentDatabase.GetInvestmentPortfolioFromDb(UserId);
            Username = Globals.CurrentDatabase.GetUsernameFromDb(UserId) ?? "invalid user";
            UserSettings = Globals.CurrentDatabase.GetSettingsFromUserSettingsDb(UserId);
            MainWindowViewModel.Timekeeping.CurrentTime = DateTimeOffset.FromUnixTimeSeconds(UserSettings.SimTime);
        }

        [Reactive] public string Username { get; set; }
        public InvestmentPortfolio UserInvestmentPortfolio { get; }
        public string ApiKey { get; set; } = "79D95RFJBZUF150U";

        public bool AutoRefresh = false;
        public Usersettings UserSettings { get; set; }

        /// <summary>
        /// Dictionary of all stocks that need to get automatic updates, and how often those updates are downloaded.
        /// This is not for realtime trading, but could possibly be used as such.
        /// </summary>
        /// <example>
        /// ETH with a intraday span of 5 min
        /// </example>
        [Reactive]
        public IList<AutoRefreshable> AutoRefreshList { get; set; } = new List<AutoRefreshable>
            { new("ETH", TimeSpan.FromSeconds(30), "5min", ImportType.Crypto) };
    }

    public class AutoRefreshable
    {
        public AutoRefreshable(string ticker, TimeSpan refreshInterval, string apiInterval,
            ImportType type = ImportType.Stock)
        {
            Ticker = ticker;
            RefreshInterval = refreshInterval;
            ApiInterval = apiInterval;
            Type = type;
        }

        public string Ticker { get; set; }
        public TimeSpan RefreshInterval { get; set; }
        public string ApiInterval { get; set; }
        public ImportType Type { get; set; }

        public void Download()
        {
            LastDownload = DateTimeOffset.Now;
            DataImport(Ticker, ApiInterval,
                Type);
        }

        public DateTimeOffset LastDownload { get; set; } = DateTimeOffset.Now;
    }
}