using System.Collections.ObjectModel;
using SoftwareProject.Types;

namespace SoftwareProject
{
    public static class Globals
    {
        public static Database CurrentDatabase = new();
        
        public static ObservableCollection<Stock> AvailableStocks { get; } = new();
    }
}