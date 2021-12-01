using System;
using System.Collections.Generic;
using System.Text;
using SoftwareProject.Models;

namespace SoftwareProject.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        // Constants for main window
        public string Intro => "Welkom bij het softwareproject!";

        // Submodels for the pages
        public HomePageViewModel HomePage { get; } = new();
        public SettingsPageViewModel SettingsPage { get; } = new();

        public DatabaseModel Database { get; } = new();
    }
}
