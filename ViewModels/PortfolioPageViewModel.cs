using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using DynamicData.Binding;
using ReactiveUI;
using SoftwareProject.Models;

namespace SoftwareProject.ViewModels
{
    public class PortfolioPageViewModel : ViewModelBase
    {
        private UserModel _userModel => MainWindowViewModel.User;

        public string Username
        {
            get => _userModel.Username;
        }

        public string TimeOfDay
        {
            get
            {
                if (DateTime.Now.Hour < 12)
                    return "Morning";
                if (DateTime.Now.Hour < 18)
                    return "Afternoon";
                return "Evening";
            }
        }

        public IObservable<string> Greeting => _userModel.WhenAny(x => x.Username, s => $"Good {TimeOfDay}, {s.Value}");

        public ObservableCollection<Investment> Investments { get; } = new() {new Investment()};
    }
}