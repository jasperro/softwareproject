using System;
using ReactiveUI;

namespace SoftwareProject.Models
{
    public class UserModel : ReactiveObject
    {
        private string _username = "bananen";
        
        public string Username
        {
            get => _username;
            set => this.RaiseAndSetIfChanged(ref _username, value);
        }
    }
}