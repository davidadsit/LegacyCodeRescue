using System;
using MusicStore.UI.Tests.PageObjects;
using OpenQA.Selenium;

namespace MusicStore.UI.Tests
{
    public class NavigationTestHelper
    {
        private readonly string baseUrl;
        private readonly IWebDriver driver;
        const string BackgroundLoginUser = "stage-test";
        const string BackgroundLoginPassword = "SVbRKUFqu9jv";

        public NavigationTestHelper(IWebDriver driver, string baseUrl)
        {
            this.driver = driver;
            this.baseUrl = baseUrl;
        }

        public HomePage HomePage()
        {
            Page("");
            try
            {
                return new HomePage(driver);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Are tests running against the correct environment? Base URL: {0}\r\n", baseUrl), ex);
            }
        }

        public string Page(string url)
        {
            driver.Navigate().GoToUrl(baseUrl + "/" + url);
            if (driver.Title == "background login")
            {
                driver.FindElement(By.Name("UserHandle")).SendKeys(BackgroundLoginUser);
                driver.FindElement(By.Name("Password")).SendKeys(BackgroundLoginPassword);
                driver.FindElement(By.Id("login")).Click();
                if (!driver.Url.EndsWith(url))
                {
                    driver.Navigate().GoToUrl(baseUrl + "/" + url);
                }
            }
            return driver.PageSource;
        }
    }
}