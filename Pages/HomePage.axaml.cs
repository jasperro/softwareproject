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

        private void InputElement_OnPointerWheelChanged(object? sender, PointerWheelEventArgs? e)
        {
            MainWindowViewModel.HomePage.FollowTicker = false;
        }

        private void DatePicker_OnSelectedDateChanged(object? sender, DatePickerSelectedValueChangedEventArgs e)
        {
            if (sender?.GetType() != typeof(DatePicker)) return;
            // The action was not initiated by the user, so we want to keep the ticker following
            if (!((DatePicker)sender).IsFocused) return;

            MainWindowViewModel.HomePage.DayByDayMode = true;
        }
    }
}