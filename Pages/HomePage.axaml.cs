using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
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
            HomePageViewModel.Series1.
        }
    }
}