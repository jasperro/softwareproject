using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SoftwareProject.Types;

namespace SoftwareProject.Models
{
    /// <summary>
    /// Stores all the information that only the current user has access to.
    /// <example>Username, Investments, User settings</example>
    /// </summary>
    public class UserModel : ReactiveObject
    {
        // Temporary user id is 0 by default
        public readonly int UserId = 0;
        
        [Reactive]
        public string Username
        {
            get;
            set;
        } = "";

        [Reactive]
        public InvestmentPortfolio UserInvestmentPortfolio
        {
            get;
            set;
        } = new();
    }

    public class InvestmentPortfolio : ObservableCollection<Investment>
    {
        public int StockAmt = 4;
        public double PortfolioTrend = 30.422;
    }
}