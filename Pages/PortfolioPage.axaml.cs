using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace SoftwareProject.Pages
{
    public class PortfolioPage : UserControl
    {
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