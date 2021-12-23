using System;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using SoftwareProject.Types;
using SoftwareProject.ViewModels;

namespace SoftwareProject.Pages
{
    public class HomePage : UserControl
    {
        private readonly HomePageViewModel _viewmodel = MainWindowViewModel.HomePage;

        public HomePage()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private int _testdaycounter = 0;

        private void AddPointButton_OnClick(object? sender, RoutedEventArgs e)
        {
            Random rnd = new Random();
            _viewmodel.Series.Last().Values = _viewmodel.Series.Last().Values?.Append(new StockPoint(
                new DateTime(2021, 1, 1).AddDays(_testdaycounter), rnd.Next(100, 1000), rnd.Next(100, 1000),
                rnd.Next(100, 1000),
                rnd.Next(100, 1000)));
            _testdaycounter++;
        }

        private void AddStockButton_OnClick(object? sender, RoutedEventArgs e)
        {
            _testdaycounter = 0;
            Stock newstock;

            try
            {
                newstock = Globals.CurrentDatabase.GetStockFromDb(_viewmodel.NewStockName);
            }
            catch
            {
                newstock = new Stock(_viewmodel.NewStockName);
            }

            _viewmodel.Stocks.Add(newstock);
            _viewmodel.Series.Add(_viewmodel.Stocks.Last());
        }
    }
}