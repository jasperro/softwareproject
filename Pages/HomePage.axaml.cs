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
        private HomePageViewModel _viewmodel = new HomePageViewModel();
        public HomePage()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            DataContext = _viewmodel;
        }

        private void AddPointButton_OnClick(object? sender, RoutedEventArgs e)
        {
            HomePageViewModel.Series1.Values = HomePageViewModel.Series1.Values.Append(new DataModel.StockPoint(new DateTime(2021, 1, 22), 630, 610, 630,
                590));
        }
    }
}