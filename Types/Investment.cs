using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using SoftwareProject.Algorithms;
using SoftwareProject.ViewModels;

namespace SoftwareProject.Types
{
    public class Investment
    {
        /// <summary>This list contains the predictive algorithms that will be applied after each other</summary>
        /// <example>This allows us to take for example the average, and apply a smoothing factor after this.</example>
        public List<IAlgorithm> AlgorithmList;

        public Stock Stock { get; }

        public Investment(Stock stock, DateTime? startOfInvestment = null)
        {
            Stock = stock;
            StartOfInvestment = startOfInvestment ?? DateTime.Now;
        }

        public Investment(string shortName, DateTime? startOfInvestment = null)
        {
            Stock = MainWindowViewModel.GlobalData.AvailableStocks.FirstOrDefault(x => x.ShortName == shortName) ??
                    Globals.CurrentDatabase.GetStockFromDb(shortName);
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
    public class InvestmentPortfolio : ObservableCollection<Investment>
    {
        public int StockAmt => 4;
        public double PortfolioTrend => 30.422;
        public int TotalProfits => 0;
        public int TotalInvested => 0;
    }
}