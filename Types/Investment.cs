using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using SoftwareProject.Algorithms;
using SoftwareProject.ViewModels;
using static SoftwareProject.Globals;

namespace SoftwareProject.Types
{
    public class Investment
    {
        /// <summary>This list contains the predictive algorithms that will be applied after each other</summary>
        /// <example>This allows us to take for example the average, and apply a smoothing factor after this.</example>
        public List<IAlgorithm> AlgorithmList = new();

        public Stock Stock { get; }

        public Investment(Stock stock, DateTime? startOfInvestment = null)
        {
            Stock = stock;
            StartOfInvestment = startOfInvestment ?? DateTime.Now;
        }

        public Investment(string shortName, DateTime? startOfInvestment = null)
        {
            Stock = GetStock(shortName);
            StartOfInvestment = startOfInvestment ?? DateTime.Now;
        }

        public string ShortName
        {
            get => Stock.ShortName;
            set => Stock.ShortName = value;
        }

        public DateTime StartOfInvestment { get; }
        public double Profit => MoneyReturn - MoneyInvested;
        public double MoneyInvested { get; set; }
        public double MoneyReturn { get; set; }
    }
    public class InvestmentPortfolio : Collection<Investment>
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

        public double TotalProfits => this.Sum(investment => investment.Profit);
        public double TotalInvested => this.Sum(investment => investment.MoneyInvested);
        public double TotalReturn => this.Sum(investment => investment.MoneyReturn);
    }
}