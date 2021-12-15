using System.Collections.ObjectModel;
using ReactiveUI;
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
    }
}