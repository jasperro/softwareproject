﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using ReactiveUI;
using SkiaSharp;
using SoftwareProject.Models;

namespace SoftwareProject.ViewModels
{
    public class SettingsPageViewModel : ViewModelBase
    {
        private UserModel _userModel => MainWindowViewModel.User;

        public string Username
        {
            get => _userModel.Username;
            set => _userModel.Username = value;
        }

        public IObservable<string> Greeting => _userModel.WhenAny(x => x.Username, s => "Uw naam is: " + s.Value);
    }
}