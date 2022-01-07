using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using SoftwareProject.Models;
using SoftwareProject.Types;
using SoftwareProject.ViewModels;

namespace SoftwareProject.Pages
{
    public class PortfolioPage : UserControl
    {
        private static readonly PortfolioPageViewModel Viewmodel = MainWindowViewModel.PortfolioPage;
        public PortfolioPage()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}