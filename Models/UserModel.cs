using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ReactiveUI;

namespace SoftwareProject.Models
{
    public class UserModel : ReactiveObject
    {
        private string _username = "bananen";
        private InvestmentPortfolio _investedstocks = new();
        
        public string Username
        {
            get => _username;
            set => this.RaiseAndSetIfChanged(ref _username, value);
        }

        public InvestmentPortfolio InvestedStocks
        {
            get => _investedstocks;
            set => this.RaiseAndSetIfChanged(ref _investedstocks, value);
        }
    }

    public class InvestmentPortfolio : ObservableCollection<Investment>
    {
        public int StockAmt = 4;
        public double PortfolioTrend = 30.422;
    }
}