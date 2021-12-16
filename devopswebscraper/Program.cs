using System.IO;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using CsvHelper;

namespace WebDriverTest
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var driver = new ChromeDriver())
            {
                string keuze;
                var csv = new StringBuilder();
            kiezen:
                Console.WriteLine("Welke site wil je scrapen?");
                Console.WriteLine("Youtube = y");
                Console.WriteLine("Indeed = i");
                Console.WriteLine("TCGMarket = m");
                Console.WriteLine("Exit application = S");
                Console.WriteLine();
                keuze = Console.ReadLine();
                while (keuze != "S")
                {
                    if (keuze == "y" || keuze == "Y")
                    {
                        youtube: 
                        Console.WriteLine("Wat wil je zoeken?:");
                        string ytZoekTerm = Console.ReadLine();
                        if (ytZoekTerm != null || ytZoekTerm != "")
                        {
                            driver.Navigate().GoToUrl("https://www.youtube.com");
                            driver.Manage().Window.Maximize();

                            try
                            {
                                var confirm = driver.FindElement(By.XPath("//*[@id=\"content\"]/div[2]/div[5]/div[2]/ytd-button-renderer[2]/a"));
                                confirm.Click();

                                var input = driver.FindElement(By.XPath("/html/body/ytd-app/div/div/ytd-masthead/div[3]/div[2]/ytd-searchbox/form/div[1]/div[1]/input"));

                                input.Click();
                                input.SendKeys(ytZoekTerm);
                                input.Submit();

                            } catch
                            {
                                var input = driver.FindElement(By.XPath("/html/body/ytd-app/div/div/ytd-masthead/div[3]/div[2]/ytd-searchbox/form/div[1]/div[1]/input"));

                                input.Click();
                                input.SendKeys(ytZoekTerm);
                                input.Submit();
                            }


                            var filter = driver.FindElement(By.XPath("//*[@id=\"container\"]/ytd-toggle-button-renderer/a"));
                            filter.Click();

                            var uploadDate = driver.FindElement(By.XPath("/html/body/ytd-app/div/ytd-page-manager/ytd-search/div[1]/ytd-two-column-search-results-renderer/div/ytd-section-list-renderer/div[1]/div[2]/ytd-search-sub-menu-renderer/div[1]/iron-collapse/div/ytd-search-filter-group-renderer[5]/ytd-search-filter-renderer[2]/a/div/yt-formatted-string"));
                            uploadDate.Click();

                            Thread.Sleep(5000);

                            try
                            {
                                var titles = driver.FindElements(By.XPath("/html/body/ytd-app/div/ytd-page-manager/ytd-search/div[1]/ytd-two-column-search-results-renderer/div/ytd-section-list-renderer/div[2]/ytd-item-section-renderer/div[3]/ytd-video-renderer/div[1]/div/div[1]/div/h3/a/yt-formatted-string"));
                                var weergaven = driver.FindElements(By.XPath("/html/body/ytd-app/div/ytd-page-manager/ytd-search/div[1]/ytd-two-column-search-results-renderer/div/ytd-section-list-renderer/div[2]/ytd-item-section-renderer/div[3]/ytd-video-renderer/div[1]/div/div[1]/ytd-video-meta-block/div[1]/div[2]/span[1]"));
                                var uploaders = driver.FindElements(By.XPath("/html/body/ytd-app/div/ytd-page-manager/ytd-search/div[1]/ytd-two-column-search-results-renderer/div/ytd-section-list-renderer/div[2]/ytd-item-section-renderer/div[3]/ytd-video-renderer/div[1]/div/div[2]/ytd-channel-name/div/div/yt-formatted-string"));
                                var links = driver.FindElements(By.XPath("/html/body/ytd-app/div/ytd-page-manager/ytd-search/div/ytd-two-column-search-results-renderer/div/ytd-section-list-renderer/div/ytd-item-section-renderer/div/ytd-video-renderer/div/div/div/div/h3/a"));
                                var i = 0;

                                while (i < 5)
                                {
                                    csv.Append("\"" + titles[i].Text + "\"");
                                    csv.Append(";");
                                    csv.Append("\"" + weergaven[i].Text + "\"");
                                    csv.Append(";");
                                    csv.Append("\"" + uploaders[i].Text + "\"");
                                    csv.Append(";");
                                    csv.Append("\"" + links[i].GetDomProperty("href") + "\"");
                                    csv.Append("\n");
                                    i++;
                                }
                                Console.WriteLine("De resultaten staan in de csv file! (Check je bureaublad!)");
                                var filepath = (Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/youtube.csv");
                                File.AppendAllText(filepath, csv.ToString());
                                goto kiezen;

                            } catch
                            {
                                Console.WriteLine("Youtube kan deze zoekterm niet vinden, probeer een andere!");
                                goto youtube;
                            }
                        }
                    }
                    else if (keuze == "i" || keuze == "I")
                    {
                        Console.WriteLine("Welke jobadvertenties wil je zien?:");
                        string indeedZoekTerm = Console.ReadLine();
                        driver.Navigate().GoToUrl("https://be.indeed.com/advanced_search?");
                        driver.Manage().Window.Maximize();

                        var optieDatum = driver.FindElement(By.XPath("/html/body/div[2]/form/fieldset[2]/div[3]/div/div[3]/select/option[2]"));
                        optieDatum.Click();

                        Thread.Sleep(500);

                        var cbxPeriode = driver.FindElement(By.XPath("/html/body/div[2]/form/fieldset[2]/div[2]/div[2]/select"));
                        cbxPeriode.Click();

                        Thread.Sleep(500);

                        var optiePeriode = driver.FindElement(By.XPath("/html/body/div[2]/form/fieldset[2]/div[2]/div[2]/select/option[4]"));
                        optiePeriode.Click();

                        Thread.Sleep(500);

                        var input = driver.FindElement(By.XPath("/html/body/div[2]/form/fieldset[1]/div[1]/div[2]/input"));
                        input.Click();
                        input.SendKeys(indeedZoekTerm);
                        input.Submit();

                        var i = 0;
                        var titels = driver.FindElements(By.XPath("/html/body/table[2]/tbody/tr/td/table/tbody/tr/td[1]/div[5]/div/a/div[1]/div/div[1]/div/table[1]/tbody/tr/td/div[1]/h2/span"));
                        var bedrijven = driver.FindElements(By.XPath("/html/body/table[2]/tbody/tr/td/table/tbody/tr/td[1]/div[5]/div/a/div[1]/div/div[1]/div/table[1]/tbody/tr/td/div[2]/pre/span[1]"));
                        var locaties = driver.FindElements(By.XPath("/html/body/table[2]/tbody/tr/td/table/tbody/tr/td[1]/div[5]/div/a/div[1]/div/div[1]/div/table[1]/tbody/tr/td/div[2]/pre/div"));
                        var links = driver.FindElements(By.XPath("/html/body/table[2]/tbody/tr/td/table/tbody/tr/td[1]/div[5]/div/a"));

                        foreach (var titel in titels)
                        {
                            csv.Append("\"" + titels[i].Text + "\"");
                            csv.Append(";");
                            csv.Append("\"" + bedrijven[i].Text + "\"");
                            csv.Append(";");
                            csv.Append("\"" + locaties[i].Text + "\"");
                            csv.Append(";");
                            csv.Append("\"" + links[i].GetDomProperty("href") + "\"");
                            csv.Append("\n");
                            i++;
                        }
                        Console.WriteLine("De resultaten staan in de csv file! (Check je bureaublad!)");
                        var filepath = (Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/indeed.csv");
                        File.AppendAllText(filepath, csv.ToString());
                        goto kiezen;


                    }
                    else if (keuze == "m" || keuze == "M")
                    {
                        Console.WriteLine("Welk product wil je zoeken?: ");
                        string ygoZoekTerm = Console.ReadLine();
                        driver.Navigate().GoToUrl("https://www.cardmarket.com/en/YuGiOh");
                        try
                        {
                            driver.Manage().Window.Maximize();
                        } catch
                        {
                            Console.WriteLine("Window is already maximized");
                        }

                        var input = driver.FindElement(By.XPath("/html/body/header/nav[2]/form/div/div/input[1]"));
                        input.Click();
                        input.SendKeys(ygoZoekTerm);
                        input.Submit();

                        var i = 0;
                        var sets = driver.FindElements(By.XPath("/html/body/main/section/div[3]/div[2]/div/div[3]/a/span/span[1]"));
                        var namen = driver.FindElements(By.XPath("/html/body/main/section/div[3]/div[2]/div/div[4]/div/div[1]/a"));
                        var rarity = driver.FindElements(By.XPath("/html/body/main/section/div[3]/div[2]/div/div[4]/div/div[2]/span/span"));
                        var available = driver.FindElements(By.XPath("/html/body/main/section/div[3]/div[2]/div/div[5]/span"));
                        var cost = driver.FindElements(By.XPath("/html/body/main/section/div[3]/div[2]/div/div[6]"));

                        csv.Append("set");
                        csv.Append(";");
                        csv.Append("naam");
                        csv.Append(";");
                        csv.Append("rarity");
                        csv.Append(";");
                        csv.Append("eenheden verkrijgbaar");
                        csv.Append(";");
                        csv.Append("prijs");
                        csv.Append(";");
                        csv.Append("link");
                        csv.Append("\n");

                        foreach (var set in sets)
                        {
                            csv.Append("\"" + sets[i].Text + "\"");
                            csv.Append(";");
                            csv.Append("\"" + namen[i].Text + "\"");
                            csv.Append(";");
                            try
                            {

                                csv.Append("\"" + rarity[i].GetDomAttribute("data-original-title") + "\"");
                            }
                            catch
                            {
                                csv.Append("Nvt");
                            }
                            csv.Append(";");
                            csv.Append("\"" + available[i].Text + "\"");
                            csv.Append(";");
                            csv.Append("\"" + cost[i].Text + "\"");
                            csv.Append(";");
                            csv.Append("\"" + namen[i].GetDomProperty("href") + "\"");
                            csv.Append("\n");
                            i++;
                        }
                        Console.WriteLine("De resultaten staan in de csv file! (Check je bureaublad!)");
                        var filepath = (Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/yugioh.csv");
                        File.AppendAllText(filepath, csv.ToString());
                        goto kiezen;
                    }
                    else if (keuze == "S")
                    {
                        driver.Close();
                        Environment.Exit(0);
                    }
                    else
                    {
                        goto kiezen;
                    }
                }
            }
        }
    }
}