using System;
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
        private readonly UserModel _model = new();

        public string Username
        {
            get => _model.Username;
            set => this.RaiseAndSetIfChanged(ref _model.Username, value);
        }

        public IObservable<string> Greeting => this.WhenAny(x => x.Username, s => "Uw naam is: " + s.Value);
    }
}
