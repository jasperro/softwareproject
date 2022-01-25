using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using DynamicData;
using DynamicData.Binding;
using LiveChartsCore.Defaults;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SoftwareProject.Algorithms;
using SoftwareProject.ViewModels;
using static SoftwareProject.Globals;

namespace SoftwareProject.Types
{
    public class Investment : ReactiveObject
    {
        /// <summary>This list contains the predictive algorithms that will be applied after each other</summary>
        /// <example>This allows us to take for example the average, and apply a smoothing factor after this.</example>
        public List<IStockAlgorithm> AlgorithmList = new();

        public Stock Stock { get; }

        public Investment(string shortName,
            double? moneyInvested = null, int amountInvested = 1, DateTime? startOfInvestment = null)
        {
            Stock = GetStock(shortName) ?? throw new InvalidOperationException();
            StartOfInvestment = startOfInvestment ?? DateTimeOffset.Now;
            moneyReturn = 
            this.WhenAnyValue(x => x.Stock.Values).Select(x => AmountInvested * x?.Last()?.Close).ToProperty(this, x => x.MoneyReturn);
            profit =
            this.WhenAnyValue(x => x.MoneyReturn).Select(x => x - MoneyInvested).ToProperty(this, x => x.Profit);
            AmountInvested = amountInvested;
            MoneyInvested = moneyInvested ?? amountInvested * Stock.Values?.Last()?.Close;
        }

        public string ShortName
        {
            get => Stock.ShortName;
            set => Stock.ShortName = value;
        }

        public DateTimeOffset StartOfInvestment { get; }
        public double? Profit => profit.Value;
        readonly ObservableAsPropertyHelper<double?> profit;
        public int AmountInvested { get; set; }
        public double? MoneyInvested { get; set; }

        public double? MoneyReturn => moneyReturn.Value;
        readonly ObservableAsPropertyHelper<double?> moneyReturn;
    }

    public class InvestmentPortfolio : ObservableCollection<Investment>
    {
        public int StockAmt => Count;

        public double AvgPortfolioTrend
        {
            get
            {
                try
                {
                    return this.Average(investment => investment.Stock.TrendPercentage);
                }
                catch
                {
                    return 0;
                }
            }
        }

        public IObservable<double?> TotalProfits => MainWindowViewModel.Timekeeping.WhenAnyObservable(x => x.ObservableTimer, x => x.ObservableTimer, (_,_) =>
            this.Sum(investment => investment.Profit)
        );

        public IObservable<double?> TotalInvested => MainWindowViewModel.Timekeeping.WhenAnyObservable(
            x => x.ObservableTimer, x => x.ObservableTimer, (_, _) =>
                this.Sum(investment => investment.MoneyInvested));

        public IObservable<double?> TotalReturn => this.ToObservableChangeSet().QueryWhenChanged(i =>
            i.Sum(investment => investment.MoneyReturn)
        );
    }
}