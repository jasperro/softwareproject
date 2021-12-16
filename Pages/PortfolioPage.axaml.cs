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
        private readonly PortfolioPageViewModel _viewmodel = MainWindowViewModel.PortfolioPage;
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
            _viewmodel.Investments.Add(new Investment(
                _viewmodel.SelectedStock));
        }

        private void SelectStock_OnClick(object? sender, RoutedEventArgs e)
        {
            _viewmodel.SelectedStock = MainWindowViewModel.GlobalData.AvailableStocks.FirstOrDefault(s =>
                s.ShortName == _viewmodel.StockToInvest);
        }
    }
}