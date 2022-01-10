using System;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using SoftwareProject.Types;
using SoftwareProject.ViewModels;
using static SoftwareProject.Globals;

namespace SoftwareProject.Pages
{
    public class HomePage : UserControl
    {
        private static readonly HomePageViewModel Viewmodel = MainWindowViewModel.HomePage;

        public HomePage()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}