using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using SoftwareProject.Models;
using SoftwareProject.ViewModels;

namespace SoftwareProject.Pages
{
    public class SettingsPage : UserControl
    {

        
        
        public SettingsPage()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }


        private void ApiImportDate_OnSelectedDateChanged(object sender, DatePickerSelectedValueChangedEventArgs e)
        {
            if (e.NewDate != null) MainWindowViewModel.SettingsPage.ImportDatum = e.NewDate.Value.Date;
        }
    }
}