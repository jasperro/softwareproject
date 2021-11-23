using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace SoftwareProject.Pages
{
    public class DashboardPage : UserControl
    {
        public DashboardPage()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}