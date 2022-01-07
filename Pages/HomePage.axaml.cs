using System;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using SoftwareProject.Types;
using SoftwareProject.ViewModels;
using static SoftwareProject.Globals;

namespace SoftwareProject.Pages
{
    public class HomePage : UserControl
    {
        private static readonly HomePageViewModel Viewmodel = MainWindowViewModel.HomePage;

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
            Viewmodel.Series.Last().Values = Viewmodel.Series.Last().Values?.Append(new StockPoint(
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
                newstock = GetStock(Viewmodel.NewStockName);
            }
            catch
            {
                newstock = new Stock(Viewmodel.NewStockName);
            }

            Viewmodel.Stocks.Add(newstock);
            Viewmodel.Series.Add(Viewmodel.Stocks.Last());
        }
    }
}