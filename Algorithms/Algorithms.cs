using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Collections;
using LiveChartsCore.Defaults;
using ReactiveUI.Fody.Helpers;
using SoftwareProject.Types;

namespace SoftwareProject.Algorithms
{
    public interface IAlgorithm
    {
        public string AlgorithmId { get; }
        public string AlgorithmName { get; }

        public IStock Apply(string shortName);
        public IStock Apply(IStock stock);
    }

    public class AverageClosingPrice : IAlgorithm
    {
        public string AlgorithmId => "avgclosing";
        public string AlgorithmName => "Average Closing Price";

        public IStock Apply(string shortName)
        {
            return Globals.CurrentDatabase.GetStockFromDb(shortName);
        }

        public IStock Apply(IStock stock)
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

        public IStock Apply(string shortName)
        {
            Stock predictedStock = Globals.CurrentDatabase.GetStockFromDb(shortName);

            DateTime date = predictedStock.LastUpdate;

            for (int i = 0; i < 100; i++)
            {
                predictedStock.Values = predictedStock.Values.Append(new FinancialPoint(date, generateRndNum(), generateRndNum(),
                    generateRndNum(), generateRndNum()));
                date.AddHours(4);
            }

            return predictedStock;
        }

        private double generateRndNum()
        {
            return (double)_rnd.Next((int)(FirstBetween * 100), (int)(SecondBetween * 100)) / 100;
        }

        public IStock Apply(IStock stock)
        {
            return Apply(stock.ShortName);
        }

        private System.Random _rnd = new();
    }

    public static class AlgorithmHelpers
    {
        /// <returns>
        /// Average closing price of a StockPoints collection
        /// </returns>
        public static double AverageClosingPrice(IEnumerable<FinancialPoint> stockPoints)
        {
            double sum = 0;
            double average;
            foreach (StockPoint stockPoint in stockPoints)
            {
                sum += stockPoint.Close;
            }

            average = sum / stockPoints.Count();
            return average;
        }

        public static IEnumerable<IAlgorithm> AlgorithmList { get; } =
            new AvaloniaList<IAlgorithm> { new AverageClosingPrice(), new Random() };
    }
}