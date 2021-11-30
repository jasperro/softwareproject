using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using SoftwareProject.Models;
using SoftwareProject.ViewModels;

namespace SoftwareProject.Pages
{
    public class HomePage : UserControl
    {
        private readonly HomePageViewModel _viewmodel = new();

        public HomePage()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            DataContext = _viewmodel;
        }

        private int _testdaycounter = 0;

        private void AddPointButton_OnClick(object? sender, RoutedEventArgs e)
        {
            Random rnd = new Random();
            _viewmodel.Series.Last().Values = _viewmodel.Series.Last().Values?.Append(new DataModel.StockPoint(
                new DateTime(2021, 1, 1).AddDays(_testdaycounter), rnd.Next(100, 1000), rnd.Next(100, 1000), rnd.Next(100, 1000),
                rnd.Next(100, 1000)));
            _testdaycounter++;
        }

        private void AddStockButton_OnClick(object? sender, RoutedEventArgs e)
        {
            _testdaycounter = 0;
            _viewmodel.Stocks.Add(new DataModel.Stock(_viewmodel.NewStockName));
            _viewmodel.Series.Add(_viewmodel.Stocks.Last());
        }
    }
}
