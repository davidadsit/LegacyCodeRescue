using System;
using System.Configuration;
using System.Diagnostics;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace MusicStore.UI.Tests
{
    public class UiTest
    {
        protected string baseUrl;
        protected FirefoxDriver driver;
        protected NavigationTestHelper navigateTo;

        [SetUp]
        public void StartBrowserSession()
        {
            driver = new FirefoxDriver();
            baseUrl = ConfigurationManager.AppSettings["siteUrl"];
            navigateTo = new NavigationTestHelper(driver, baseUrl);
        }

        [TearDown]
        public void EndBrowserSession()
        {
            try
            {
                driver.Quit();
            }
            catch (Exception)
            {
                // Ignore errors if unable to close the browser
            }
        }

        protected void AssertSoon(Func<bool> func, string message = "", int maxSeconds = 5)
        {
            var stopwatch = Stopwatch.StartNew();
            var conditionMet = func();
            while (stopwatch.ElapsedMilliseconds < maxSeconds*1000 && !conditionMet)
            {
                Thread.Sleep(100);
                conditionMet = func();
            }
            Assert.IsTrue(conditionMet, message);
        }
    }
}