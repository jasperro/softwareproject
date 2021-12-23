using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using SoftwareProject.Models;
using SoftwareProject.Types;
using SoftwareProject.ViewModels;

namespace SoftwareProject.Pages
{
    public class PortfolioPage : UserControl
    {
        private static readonly PortfolioPageViewModel Viewmodel = MainWindowViewModel.PortfolioPage;
        public PortfolioPage()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void AddInvest_OnClick(object? sender, RoutedEventArgs e)
        {
            Investment newInvestment = new(Viewmodel.SelectedStock);
            Viewmodel.Investments.Add(newInvestment);
            Globals.CurrentDatabase.AddInvestmentToDb(MainWindowViewModel.User.UserId, newInvestment);
        }

        private void SelectStock_OnClick(object? sender, RoutedEventArgs e)
        {
            Viewmodel.SelectedStock = MainWindowViewModel.GlobalData.AvailableStocks.FirstOrDefault(s =>
                s.ShortName == Viewmodel.StockToInvest);
        }
    }
}