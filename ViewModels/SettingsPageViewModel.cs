using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Avalonia.Controls;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SkiaSharp;
using SoftwareProject.Models;
using SoftwareProject.Pages;

namespace SoftwareProject.ViewModels
{
    public class SettingsPageViewModel : ViewModelBase
    {
        private UserModel _userModel => MainWindowViewModel.User;
        private string ticker;
        
        private GlobalDataModel _globalData => MainWindowViewModel.GlobalData;

        public string Username
        {
            get => _userModel.Username;
            set => _userModel.Username = value;
        }

        public IObservable<string> Greeting => _userModel.WhenAny(x => x.Username, s => "Uw naam is: " + s.Value);

        public string Ticker
        {
            set { ticker = value; }
            get { return ticker; }
        }
        
        
        
        public ComboBoxItem CbbItem
        {
            set; get;
        }

        public string? Interval => CbbItem.Content.ToString();

        public double UpdateTimeMultiplier
        {
            get => _globalData.UpdateTimeMultiplier;
            set => _globalData.UpdateTimeMultiplier = value;
        }

        [Reactive]
        public DateTime ImportDatum { get; set; }

        public void ApiImportButton()
        {
            ApiModel.DataImport(ticker, ImportDatum, Interval);
        }
        
    }
}