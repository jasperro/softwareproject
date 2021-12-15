using System;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SoftwareProject.Models;
using SoftwareProject.Types;
using SoftwareProject.ViewModels;
using SoftwareProject.Views;

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
                new DateTime(2021, 1, 1).AddDays(_testdaycounter), rnd.Next(100, 1000), rnd.Next(100, 1000), rnd.Next(100, 1000),
                rnd.Next(100, 1000)));
            _testdaycounter++;
        }

        private void AddStockButton_OnClick(object? sender, RoutedEventArgs e)
        {
            _testdaycounter = 0;
            _viewmodel.Stocks.Add(new Stock(_viewmodel.NewStockName));
            _viewmodel.Series.Add(_viewmodel.Stocks.Last());
        }
    }
}
