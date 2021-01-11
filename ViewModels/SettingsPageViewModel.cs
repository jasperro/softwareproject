﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Avalonia.Controls;
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

        private TimekeepingModel _timeKeeping => MainWindowViewModel.Timekeeping;

        public string Username
        {
            get => _userModel.Username;
            set => _userModel.Username = value;
        }

        public IObservable<string> Greeting => _userModel.WhenAny(x => x.Username, s => "Uw naam is: " + s.Value);

        public string Ticker { set; get; } = "";

        public ComboBoxItem? CbbItem
        {
            set; get;
        }

        private string Interval => CbbItem?.Content.ToString() ?? "daily";

        [Reactive] public DateTimeOffset ImportDatum { get; set; } = DateTimeOffset.Now;

        public void ApiImportButton()
        {
            ApiModel.DataImport(Ticker, ImportDatum, Interval);
        }
        
    }
}