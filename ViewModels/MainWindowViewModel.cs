using System;
using System.Collections.Generic;
using System.Text;

namespace SoftwareProject.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public string Intro => "Welkom bij het softwareproject!";
        public HomePageViewModel HomePage { get; } = new();
        public SettingsPageViewModel SettingsPage { get; } = new();
    }
}