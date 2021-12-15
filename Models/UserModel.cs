using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ReactiveUI;
using SoftwareProject.Types;

namespace SoftwareProject.Models
{
    /// <summary>
    /// Stores all the information that only the current user has access to.
    /// <example>Username, Investments, User settings</example>
    /// </summary>
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