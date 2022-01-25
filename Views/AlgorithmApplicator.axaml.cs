using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace SoftwareProject.Views
{
    public class AlgorithmApplicator : Window
    {
        public AlgorithmApplicator()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}