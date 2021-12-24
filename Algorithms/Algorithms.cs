using System.Collections.Generic;
using Avalonia.Collections;
using DynamicData;
using SoftwareProject.Types;

namespace SoftwareProject
{
    public interface IAlgorithm
    {
        public string AlgorithmId { get; }
        public string AlgorithmName { get; }

        public IStock Apply(string shortName);
        public IStock Apply(IStock stock);
    }
    
    public static class Algorithms
    {
        /*
        public double AverageClosingPrice(ObservableCollection<FinancialPoint> stockPoints)
        // returns average closing price of StockPoints collection
        /*
        {
            double sum = 0;
            double average;
            foreach (StockPoint stockPoint in stockPoints)
            {
                sum += stockPoint.Close;
            }
            average = sum / stockPoints.Count;
            return average;

        }
        */
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
        
        public static IEnumerable<IAlgorithm> AlgorithmList { get; } =
            new AvaloniaList<IAlgorithm> { new AverageClosingPrice() };
        
    }
}