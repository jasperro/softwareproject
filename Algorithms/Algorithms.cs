using System.Collections.Generic;
using Avalonia.Collections;
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
        
        // For some stupid reason, AvaloniaDictionary did not work due to not implementing IList.
        public static IEnumerable<KeyValuePair<string, IAlgorithm>> AlgorithmList { get; } =
            new AvaloniaList<KeyValuePair<string, IAlgorithm>> { new("Average", new AverageClosingPrice()) };
        
    }
}