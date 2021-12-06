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
        public static HomePageViewModel HomePage { get; } = new();
        public static SettingsPageViewModel SettingsPage { get; } = new();

        public static DatabaseModel Database { get; } = new();
    }
}
