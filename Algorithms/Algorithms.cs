using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Collections;
using OxyPlot.Series;
using ReactiveUI.Fody.Helpers;
using SkiaSharp;
using SoftwareProject.Types;
using static SoftwareProject.Globals;

namespace SoftwareProject.Algorithms
{
    public interface IAlgorithm
    {
        public string AlgorithmId { get; }
        public string AlgorithmName { get; }
        public Stock Apply(string shortName) => Apply(GetStock(shortName));
        public Stock Apply(Stock stock);
    }

    public class AverageClosingPrice : IAlgorithm
    {
        public string AlgorithmId => "avgclosing";
        public string AlgorithmName => "Average Closing Price";

        public Stock Apply(string shortName)
        {
            return GetStock(shortName);
        }

        public Stock Apply(Stock stock)
        {
            return Apply(stock.ShortName);
        }
    }

    /// <summary>
    /// Algorithm that will just generate random values as predictions, useful for testing.
    /// </summary>
    public class Random : IAlgorithm
    {
        public string AlgorithmId => "random";
        public string AlgorithmName => "Random Data";

        [Reactive] public double FirstBetween { get; set; } = 100;
        [Reactive] public double SecondBetween { get; set; } = 200;

        private double generateRndNum()
        {
            return (double)_rnd.Next((int)(FirstBetween * 100), (int)(SecondBetween * 100)) / 100;
        }

        public Stock Apply(Stock stock)
        {
            Stock predictedStock = new($"{stock.ShortName} (prediction)")
            {
            };

            DateTime date = stock.LastUpdate;

            for (int i = 0; i < 100; i++)
            {
                /*predictedStock.ItemsSource = predictedStock.Items.Append(new FinancialPoint(date, generateRndNum(),
                    generateRndNum(),
                    generateRndNum(), generateRndNum()));
                date = date.AddHours(4);*/
            }

            return predictedStock;
        }

        private readonly System.Random _rnd = new();
    }

    public static class AlgorithmHelpers
    {
        /// <returns>
        /// Average closing price of a StockPoints collection
        /// </returns>
        public static double AverageClosingPrice(HighLowSeries stockPoints)
        {
            double sum = 0;
            double average;
            foreach (HighLowItem stockPoint in stockPoints.Items)
            {
                sum += stockPoint.Close;
            }

            average = sum / stockPoints.Items.Count;
            return average;
        }

        public static IEnumerable<IAlgorithm> AlgorithmList { get; } =
            new AvaloniaList<IAlgorithm> { new AverageClosingPrice(), new Random() };
    }
}