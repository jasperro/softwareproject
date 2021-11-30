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
using ReactiveUI;
using SoftwareProject.Models;

namespace SoftwareProject.ViewModels
{
    public class HomePageViewModel : ViewModelBase
    {
        private string _newstockname;

        /// <summary>Stocks that are followed</summary>
        public ObservableCollection<DataModel.Stock> Stocks { get; }

        /// <summary>Stocks that are visible in the chart</summary>
        public ObservableCollection<DataModel.Stock> Series { get; set; }

        public HomePageViewModel()
        {
            Stocks = new ObservableCollection<DataModel.Stock> {new()};
            Series = new ObservableCollection<DataModel.Stock> {new()};
        }
        public string NewStockName
        {
            get => _newstockname;
            set => this.RaiseAndSetIfChanged(ref _newstockname, value);
        }

        public Axis[] XAxes { get; set; } = new[]
        {
            new Axis
            {
                LabelsRotation = 15,
                Labeler = value => new DateTime((long) value).ToString("yyyy MMM dd"),
                UnitWidth = TimeSpan.FromDays(1).Ticks
            }
        };
    }
}
