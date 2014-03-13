using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace MusicStore.UI.Tests
{
    public static class DriverExtensions
    {
        public static void SwitchToWindowByPartialTitle(this IWebDriver driver, string title)
        {
            foreach (var windowHandle in driver.WindowHandles)
            {
                driver.SwitchTo().Window(windowHandle);
                if (driver.Title.Contains(title))
                    break;
            }
        }

        public static IWebElement FindElement(this IWebDriver driver, By by, int timeoutInSeconds)
        {
            if (timeoutInSeconds > 0)
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
                return wait.Until(drv => drv.FindElement(by));
            }
            return driver.FindElement(by);
        }
    }
}