﻿using System;
using System.Collections.Generic;
using System.Text;
using SoftwareProject.Models;

namespace SoftwareProject.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        // Constants for main window
        public MainWindowViewModel()
        { 
            GlobalData.AvailableStocks.Add(Database.GetStockFromDb("AAPL"));
        }

        public string Intro => "Welkom bij het softwareproject!";
        
        // Initialize the Models
        public static DatabaseModel Database { get; } = new();
        public static UserModel User { get; }= new();
        public static GlobalDataModel GlobalData { get; } = new();

        // Sub-viewmodels for the pages
        public static HomePageViewModel HomePage { get; } = new();
        public static PortfolioPageViewModel PortfolioPage { get; } = new();
        public static SettingsPageViewModel SettingsPage { get; } = new();

    }
}
