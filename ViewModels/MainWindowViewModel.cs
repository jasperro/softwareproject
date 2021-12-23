using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Data.Sqlite;
using SoftwareProject.Models;

namespace SoftwareProject.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public string Intro => "Welkom bij het softwareproject!";
        
        // Initialize the Models
        public static UserModel User { get; }= new();
        public static GlobalDataModel GlobalData { get; } = new();

        // Sub-viewmodels for the pages
        public static HomePageViewModel HomePage { get; } = new();
        public static PortfolioPageViewModel PortfolioPage { get; } = new();
        public static SettingsPageViewModel SettingsPage { get; } = new();
    }
}
