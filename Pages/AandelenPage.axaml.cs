using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace SoftwareProject.Pages
{
    public class AandelenPage : UserControl
    {
        public AandelenPage()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}