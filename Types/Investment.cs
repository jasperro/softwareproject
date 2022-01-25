using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ReactiveUI;
using DynamicData;
using DynamicData.Binding;
using ReactiveUI.Fody.Helpers;
using SoftwareProject.Algorithms;
using SoftwareProject.ViewModels;
using static SoftwareProject.Globals;

namespace SoftwareProject.Types
{
    public class Investment
    {
        /// <summary>This list contains the predictive algorithms that will be applied after each other</summary>
        /// <example>This allows us to take for example the average, and apply a smoothing factor after this.</example>
        public List<IStockAlgorithm> AlgorithmList = new();

        public Stock Stock { get; }

        public Investment(string shortName, DateTime? startOfInvestment = null, int amountInvested = 1)
        {
            Stock = GetStock(shortName) ?? throw new InvalidOperationException();
            StartOfInvestment = startOfInvestment ?? MainWindowViewModel.Timekeeping.CurrentTime;
            AmountInvested = amountInvested;
            MoneyInvested = amountInvested * Stock.Values?.Last()?.Close;
        }

        public string ShortName
        {
            get => Stock.ShortName;
            set => Stock.ShortName = value;
        }

        public DateTimeOffset StartOfInvestment { get; }
        public IObservable<double?> Profit => this.WhenAnyObservable(x => x.MoneyReturn, x => x.MoneyReturn, (d, arg2) => d - MoneyInvested);
        public int AmountInvested { get; set; }
        public double? MoneyInvested { get; set; }

        public IObservable<double?> MoneyReturn => this.WhenAny(x => x.Stock.Values, s => s.Value?.Last()?.Close);
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

        public IObservable<double?> TotalProfits => this.ToObservableChangeSet().QueryWhenChanged(i =>
            i.Sum(investment => investment.Profit)
        );

        public IObservable<double?> TotalInvested => this.ToObservableChangeSet().QueryWhenChanged(i =>
            i.Sum(investment => investment.MoneyInvested)
        );

        public IObservable<double> TotalReturn => this.ToObservableChangeSet().QueryWhenChanged(i =>
            i.Sum(investment => investment.MoneyReturn)
        );
    }
}