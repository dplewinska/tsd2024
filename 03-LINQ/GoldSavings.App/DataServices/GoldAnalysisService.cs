using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.IO; // Missing import

using GoldSavings.App.Model;

namespace GoldSavings.App.Services
{
    public class GoldAnalysisService
    {
        private readonly List<GoldPrice> _goldPrices;

        public GoldAnalysisService(List<GoldPrice> goldPrices)
        {
            _goldPrices = goldPrices;
        }

        // Get the average gold price
        public double GetAveragePrice()
        {
            return _goldPrices.Average(p => p.Price);
        }

        // Get Top 3 highest and lowest gold prices within the last year
        public (List<GoldPrice> highest, List<GoldPrice> lowest) GetTop3Prices()
        {
            // Filter prices for the year 2024
            var goldPrices2024 = _goldPrices.Where(p => p.Date.Year == 2024).ToList();

            var top3HighestPrices = goldPrices2024.OrderByDescending(p => p.Price).Take(3).ToList();
            var top3LowestPrices = goldPrices2024.OrderBy(p => p.Price).Take(3).ToList();

            return (top3HighestPrices, top3LowestPrices);
        }


        // Check if buying in January 2020 could yield more than 5%
        public List<(DateTime Date, double Return)> GetProfitableDaysFrom2020()
        {
            DateTime january2020 = new DateTime(2020, 1, 1);
            DateTime february2020End = new DateTime(2020, 2, 29); // End of February 2020

            // Select the lowest price in or after January 2020 and before March 2020
            double initialPrice = _goldPrices
                .Where(p => p.Date >= january2020 && p.Date <= february2020End)
                .OrderBy(p => p.Price)
                .FirstOrDefault()?.Price ?? 0;

            if (initialPrice == 0)
                return new List<(DateTime, double)>(); // Return empty list if no data found

            return _goldPrices
                .Where(p => p.Date >= january2020 && p.Date <= february2020End)
                .Select(p => (p.Date, Return: (p.Price - initialPrice) / initialPrice * 100))
                .Where(r => r.Return > 5)
                .ToList();
        }

        // Get the 3 dates in 2019-2022 that open the second ten of the prices ranking
        public List<DateTime> GetSecondTenPricesDates()
        {
            return _goldPrices
                .Where(p => p.Date.Year >= 2019 && p.Date.Year <= 2022)
                .OrderByDescending(p => p.Price)
                .Skip(10)
                .Take(3)
                .Select(p => p.Date)
                .ToList();
        }

        // Get the average gold price for specific years
        public Dictionary<int, double> GetYearlyAverages(int[] years)
        {
            return years.ToDictionary(
                year => year,
                year => _goldPrices.Where(p => p.Date.Year == year).Any()
                    ? _goldPrices.Where(p => p.Date.Year == year).Average(p => p.Price)
                    : 0 // Default value to handle empty cases
            );
        }

        // Find the best buy-sell pair and return on investment
        public (DateTime BuyDate, DateTime SellDate, double Return)? GetBestInvestmentPeriod()
        {
            var bestInvestment = _goldPrices
                .SelectMany(buy => _goldPrices.Select(sell => new
                {
                    BuyDate = buy.Date,
                    SellDate = sell.Date,
                    Return = (sell.Price - buy.Price) / buy.Price * 100
                }))
                .Where(r => r.BuyDate < r.SellDate)
                .OrderByDescending(r => r.Return)
                .FirstOrDefault();

            return bestInvestment != null ? (bestInvestment.BuyDate, bestInvestment.SellDate, bestInvestment.Return) : (DateTime.MinValue, DateTime.MinValue, 0);
        }

        // Save prices to XML
        public void SavePricesToXml(string filePath)
        {
            var xml = new XElement("GoldPrices",
                _goldPrices.Select(p => 
                    new XElement("GoldPrice",
                        new XElement("Date", p.Date.ToString("yyyy-MM-dd")),
                        new XElement("Price", p.Price)
                    )
                )
            );

            xml.Save(filePath);
        }

        // Load prices from XML using a single semicolon
        public List<GoldPrice> LoadPricesFromXml(string filePath) => 
            File.Exists(filePath) ? 
                XDocument.Load(filePath).Root.Elements("GoldPrice")
                .Select(x => new GoldPrice
                {
                    Date = DateTime.Parse(x.Element("Date").Value),
                    Price = double.Parse(x.Element("Price").Value)
                }).ToList() : new List<GoldPrice>();
    } 
}
