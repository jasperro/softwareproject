using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using Avalonia;
using DynamicData.Binding;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SoftwareProject.Models;
using SoftwareProject.Types;
using static SoftwareProject.Globals;

namespace SoftwareProject.ViewModels
{
    public class PortfolioPageViewModel : ViewModelBase
    {
        [Reactive]
        private int SelectedWeek { get; set; } =
            ISOWeek.GetWeekOfYear(MainWindowViewModel.Timekeeping.CurrentTime);

        private int CurrentWeek => ISOWeek.GetWeekOfYear(MainWindowViewModel.Timekeeping.CurrentTime);


        private UserModel _userModel => MainWindowViewModel.User;

        public string Username
        {
            get => _userModel.Username;
        }

        public string TimeOfDay
        {
            get
            {
                if (DateTime.Now.Hour < 12)
                    return "Morning";
                if (DateTime.Now.Hour < 18)
                    return "Afternoon";
                return "Evening";
            }
        }

        public IObservable<string> Greeting => _userModel.WhenAny(x => x.Username, s => $"Good {TimeOfDay}, {s.Value}");

        public IObservable<string> InvestedStocksSummary => _userModel.WhenAny(x => x.UserInvestmentPortfolio,
            s => $"Your {s.Value.StockAmt} stocks have changed with {s.Value.AvgPortfolioTrend}% since yesterday");

        public InvestmentPortfolio Investments { get; } = MainWindowViewModel.User.UserInvestmentPortfolio;

        public ObservableCollection<ISeries> SharePieChart { get; } = new()
        {
            new PieSeries<ObservableValue> { Values = new[] { new ObservableValue(3) }, Name = "AAPL" },
            new PieSeries<ObservableValue> { Values = new[] { new ObservableValue(4) }, Name = "IBM" },
            new PieSeries<ObservableValue> { Values = new[] { new ObservableValue(2) }, Name = "GOOGL" }
        };

        public string StockToInvest { get; set; } = "AAPL";


        [Reactive] public Stock? SelectedStock { get; set; }

        [Reactive] public double AmountToInvest { get; set; }

        public IObservable<string> SelectedWeekInfo => this.WhenAny(x => x.SelectedWeek,
            s =>
                $"Week {s.Value}");

        public void SelectPreviousWeek()
        {
            SelectedWeek--;
        }

        public void SelectNextWeek()
        {
            SelectedWeek++;
        }

        public void SelectCurrentWeek()
        {
            SelectedWeek = CurrentWeek;
        }


        public void AddInvestment()
        {
            Investment newInvestment = new(SelectedStock);
            Investments.Add(newInvestment);
            CurrentDatabase.AddInvestmentToDb(MainWindowViewModel.User.UserId, newInvestment);
        }

        public void SelectStock()
        {
            SelectedStock = GetStock(StockToInvest);
        }
    }
}