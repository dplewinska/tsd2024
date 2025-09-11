using GoldSavings.App.Model;
using GoldSavings.App.Client;
using GoldSavings.App.Services;
namespace GoldSavings.App;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello, Gold Investor!");

        // Step 1: Get gold prices
        GoldDataService dataService = new GoldDataService();
        List<GoldPrice> goldPrices = new List<GoldPrice>();
        
        for (int year = 2020; year <= DateTime.Now.Year; year++)
        {
            var startDate = new DateTime(year, 01, 01);
            var endDate = new DateTime(year, 12, 31);
            goldPrices.AddRange(dataService.GetGoldPrices(startDate, endDate).GetAwaiter().GetResult());
        }

        if (goldPrices.Count == 0)
        {
            Console.WriteLine("No data found. Exiting.");
            return;
        }

        Console.WriteLine($"Retrieved {goldPrices.Count} records. Ready for analysis.");

        // Step 2: Perform analysis
        GoldAnalysisService analysisService = new GoldAnalysisService(goldPrices);
        var avgPrice = analysisService.GetAveragePrice();
        var (top3Highest, top3Lowest) = analysisService.GetTop3Prices();
        var profitableDays = analysisService.GetProfitableDaysFrom2020();
        var secondTenDates = analysisService.GetSecondTenPricesDates();
        var yearlyAverages = analysisService.GetYearlyAverages(new[] { 2020, 2023, 2024 });
        var bestInvestment = analysisService.GetBestInvestmentPeriod();

        // Step 3: Print results
        GoldResultPrinter.PrintSingleValue(Math.Round(avgPrice, 2), "Average Gold Price");

        Console.WriteLine("\nTop 3 Highest Prices:");
        foreach (var price in top3Highest)
            Console.WriteLine($"{price.Date.ToShortDateString()}: {price.Price}");

        Console.WriteLine("\nTop 3 Lowest Prices:");
        foreach (var price in top3Lowest)
            Console.WriteLine($"{price.Date.ToShortDateString()}: {price.Price}");

        Console.WriteLine("\nProfitable Days (More than 5% Gain):");
        foreach (var day in profitableDays)
            Console.WriteLine($"{day.Date.ToShortDateString()}: {day.Return:F2}%");

        Console.WriteLine("\nSecond Ten Prices Dates:");
        if (secondTenDates.Any())
        {
            foreach (var date in secondTenDates)
                Console.WriteLine(date.ToShortDateString());
        }
        else
        {
            Console.WriteLine("No sufficient data found to determine the second ten prices ranking.");
        }

        Console.WriteLine("\nYearly Averages:");
        foreach (var year in yearlyAverages)
            Console.WriteLine($"{year.Key}: {year.Value:F2}");

        if (bestInvestment.HasValue)
        {
            Console.WriteLine("\nBest Investment Opportunity:");
            Console.WriteLine($"Buy on {bestInvestment.Value.BuyDate.ToShortDateString()} and sell on {bestInvestment.Value.SellDate.ToShortDateString()} with a return of {bestInvestment.Value.Return:F2}%");
        }

        Console.WriteLine("\nGold Analysis Queries with LINQ Completed.");

        string filePath = "gold_prices.xml";
        analysisService.SavePricesToXml(filePath);
        Console.WriteLine("Gold prices saved to XML.");

        List<GoldPrice> loadedPrices = analysisService.LoadPricesFromXml(filePath);
        Console.WriteLine($"Loaded {loadedPrices.Count} gold prices from XML.");
    }
}
