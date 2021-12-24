using Avalonia.Controls;

namespace SoftwareProject.ViewModels
{
    public class AlgorithmApplicatorViewModel : ViewModelBase
    {
        public string ShortName
        {
            get;
        } = "";

        public CalendarDateRange? DateRange
        {
            get;
        }
        
        public AlgorithmApplicatorViewModel()
        {
        }

        public AlgorithmApplicatorViewModel(string shortName, CalendarDateRange dateRange)
        {
            ShortName = shortName;
            DateRange = dateRange;
        }
    }
}