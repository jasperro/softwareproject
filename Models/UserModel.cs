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
        public int UserId { get; }

        public UserModel(int userId = 0)
        {
            UserId = userId;
            UserInvestmentPortfolio =
                Globals.CurrentDatabase.GetInvestmentPortfolioFromDb(UserId);
        }

        [Reactive]
        public string Username
        {
            get;
            set;
        } = "";

        public InvestmentPortfolio UserInvestmentPortfolio
        {
            get;
        }
    }
}