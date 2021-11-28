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
        private UserModel model = new();
        
        public string Username
        {
            get => model.Username;
            set => this.RaiseAndSetIfChanged(ref model.Username, value);
        }

        public IObservable<string> Greeting => this.WhenAny(x => x.Username, s => "Uw naam is: " + s.Value);
    }
}
