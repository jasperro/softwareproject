using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using SoftwareProject.ViewModels;

namespace SoftwareProject.Views
{
    public class MainWindow : Window
    {
        public static MainWindowViewModel ViewModel { get; } = new();
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            
            DataContext = ViewModel;
        }
    }
}