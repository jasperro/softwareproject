using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SoftwareProject.Models;
using SoftwareProject.Types;
using static SoftwareProject.Globals;
using static SoftwareProject.ViewModels.MainWindowViewModel;

namespace SoftwareProject.ViewModels
{
    public class PortfolioPageViewModel : ViewModelBase
    {
        public PortfolioPageViewModel()
        {
            this.ObservableForProperty(x => x.SelectedStockListItem).Subscribe(_ =>
            {
                SelectedStock = SelectedStockListItem;
                StockToInvest = SelectedStock!.ShortName;
            });
            
        }

        [Reactive]
        private int SelectedWeek { get; set; } =
            ISOWeek.GetWeekOfYear(Timekeeping.CurrentTime.DateTime);

        private int CurrentWeek => ISOWeek.GetWeekOfYear(Timekeeping.CurrentTime.DateTime);


        private UserModel _userModel => User;

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

        public InvestmentPortfolio Investments { get; } = User.UserInvestmentPortfolio;

        public ObservableCollection<ISeries> SharePieChart { get; } = new()
        {
            new PieSeries<ObservableValue> { Values = new[] { new ObservableValue(3) }, Name = "AAPL" },
            new PieSeries<ObservableValue> { Values = new[] { new ObservableValue(4) }, Name = "IBM" },
            new PieSeries<ObservableValue> { Values = new[] { new ObservableValue(2) }, Name = "GOOGL" }
        };

        [Reactive] public string StockToInvest { get; set; }

        [Reactive] public Stock? SelectedStock { get; set; }

        [Reactive] public double AmountToInvest { get; set; }

        [Reactive] public Stock? SelectedStockListItem { get; set; }

        public static IEnumerable<IStock> StockList => CachedStocks;

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

        public void PreviewStock(Stock stock)
        {
            MainWindow.SelectedIndex = 0;
            HomePage.Series.Clear();
            HomePage.Series.Add(stock);
            Console.WriteLine($"Preview {stock.ShortName}");
        }


        public void AddInvestment()
        {
            if (SelectedStock == null) return;
            Investment newInvestment = new(SelectedStock) { MoneyInvested = AmountToInvest };
            Investments.Add(newInvestment);
            CurrentDatabase.AddInvestmentToDb(User.UserId, newInvestment);
        }

        public void SelectStock()
        {
            SelectedStock = GetStock(StockToInvest);
        }
    }
}