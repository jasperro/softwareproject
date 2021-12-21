using System;
using System.Collections.ObjectModel;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SoftwareProject.Types;
using SoftwareProject.ViewModels;

namespace SoftwareProject.Models
{
    /// <summary>
    /// Stores data that can be accessed by multiple users
    /// <example>Available stocks, news info, system information</example>
    /// </summary>
    public class GlobalDataModel : ReactiveObject
    {
        public ObservableCollection<Stock> AvailableStocks { get; } = new();
        
        /// <summary>
        /// The current time of the primary simulation,
        /// the point where we can look at profits from historical data
        /// </summary>
        [Reactive]
        public DateTime CurrentTime { get; set; } = DateTime.Now;

        /// <summary>
        /// The speed at which time is moving. 1.0 is normal speed (one second per second)
        /// </summary>
        [Reactive]
        public double TimeMultiplier { get; set; } = 1.0;
    }
}