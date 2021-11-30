using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using SoftwareProject.Models;

namespace SoftwareProject.ViewModels
{
    public class HomePageViewModel : ViewModelBase
    {

        public ObservableCollection<DataModel.Stock> Stocks;
        public ObservableCollection<ISeries<FinancialPoint>> Series { get; set; }

        // we have to let the chart know that the X axis in days.
        public HomePageViewModel()
        {
            Stocks = new() { new DataModel.Stock() };
            Series = new ObservableCollection<ISeries<FinancialPoint>> { };
        }

        public Axis[] XAxes { get; set; } = new[]
        {
            new Axis
            {
                LabelsRotation = 15,
                Labeler = value => new DateTime((long) value).ToString("yyyy MMM dd"),
                // set the unit width of the axis to "days"
                // since our X axis is of type date time and
                // the interval between our points is in days
                UnitWidth = TimeSpan.FromDays(1).Ticks
            }
        };


    }
}
