using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using SoftwareProject.ViewModels;

namespace SoftwareProject.Pages
{
    public class HomePage : UserControl
    {
        public HomePage()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void InputElement_OnPointerWheelChanged(object? sender, PointerWheelEventArgs e)
        {
            MainWindowViewModel.HomePage.FollowTicker = false;
        }
    }
}