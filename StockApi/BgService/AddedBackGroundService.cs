using Microsoft.EntityFrameworkCore;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using StockApi.Data;
using StockApi.Model;
using System;

namespace StockApi.BgService
{
    public class AddedBackGroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var options = new ChromeOptions();
            options.AddArgument("--headless");
            //options.AddArgument("--incognito");

            using var driver = new ChromeDriver(options);
            driver.Navigate().GoToUrl("https://tr.tradingview.com/markets/stocks-turkey/market-movers-all-stocks/");

            await Task.Delay(15000, stoppingToken);

            try
            {

                for (int i = 0; i < 5; i++)
                {
                    var loadbutton = driver.FindElement(By.CssSelector("#js-category-content > div.js-base-screener-page-component-root > div > div.root-cFX_j1gd > div.loadMoreWrapper-YZEOoLh1 > button > span "));
                    loadbutton?.Click();
                    await Task.Delay(3000, stoppingToken);


                }

            }
            catch (NoSuchElementException)
            {

                Console.WriteLine("Load button not found.Continuing...");
            }
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _serviceProvider.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                var tBodyElements = driver.FindElements(By.XPath(   
                    "//table/tbody"));

                foreach (var tbodyElement in tBodyElements)
                {
                    var trElements = tbodyElement.FindElements(By.ClassName("listRow"));

                    foreach (var tr in trElements)
                    {
                        try
                        {
                            var stock = new Stock
                            {
                                Code = tr.FindElement(By.CssSelector("td:nth-child(1) a")).Text,
                                CompanyName = tr.FindElement(By.CssSelector("td:nth-child(1) sup")).Text,
                                Price = tr.FindElement(By.CssSelector("td:nth-child(2)")).Text,
                                PercentageOfChange = tr.FindElement(By.CssSelector("td:nth-child(3) span")).Text,
                                ChangePrice = tr.FindElement(By.CssSelector("td:nth-child(4) span")).Text
                            };

                            context.stocks.Add(stock);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error parsing stock row: {ex.Message}");
                        }
                    }
                }

                await context.SaveChangesAsync(stoppingToken);
                Console.WriteLine("✔ Stocks saved to DB");

                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken); // 5 dakika bekle
                driver.Navigate().Refresh();
                await Task.Delay(10000, stoppingToken);
            }

            driver.Quit();
        }

    }


}


