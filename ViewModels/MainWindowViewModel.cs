using System;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SoftwareProject.Models;

namespace SoftwareProject.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public string Intro => "Welkom bij het softwareproject!";
        
        // Initialize the Models
        public static TimekeepingModel Timekeeping { get; } = new();
        public static UserModel User { get; } = new();

        // Sub-viewmodels for the pages
        public static HomePageViewModel HomePage { get; } = new();
        public static PortfolioPageViewModel PortfolioPage { get; } = new();
        public static SettingsPageViewModel SettingsPage { get; } = new();

        [Reactive]
        public int SelectedIndex
        {
            get;
            set;
        }
        
        [Reactive]
        public bool SmallSidebar
        {
            get;
            set;
        }
    }
}
