using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;

namespace MusicStore.UI.Tests
{
    public class PageObject
    {
        protected readonly IWebDriver driver;
        const int timeoutInSeconds = 1;

        protected PageObject(IWebDriver driver)
        {
            this.driver = driver;
            EnsureThatThePageObjectIsCorrectForTheCurrentPage();
        }

        void EnsureThatThePageObjectIsCorrectForTheCurrentPage()
        {
            AssertSoon(() => GetActualPageObjectTag() == GetExpectedPageObjectName(),
                       string.Format("Expected page '{0}' but was '{1}'. Have you added ViewBag.PageObjectTag = \"{0}\"; to the view?", GetExpectedPageObjectName(), GetActualPageObjectTag()));
        }

        string GetExpectedPageObjectName()
        {
            var expectedPageObjectName = GetType().Name;
            return expectedPageObjectName;
        }

        string GetActualPageObjectTag()
        {
            try
            {
                driver.FindElement(By.Id("pageObjectTag"), 30);
            }
            catch (WebDriverTimeoutException)
            {
                return "[UNKNOWN]";
            }
            if (!DoesElementWithIdExist("pageObjectTag")) return "[UNKNOWN]";
            var actualPageObjectTag = FindSingleElementById("pageObjectTag").GetAttribute("value");
            return string.IsNullOrEmpty(actualPageObjectTag) ? "[UNKNOWN]" : actualPageObjectTag;
        }

        protected bool DoesElementWithIdExist(string id)
        {
            return driver.FindElements(By.Id(id)).Count > 0;
        }

        protected IWebElement FindSingleElementById(string id)
        {
            return driver.FindElement(By.Id(id), timeoutInSeconds);
        }

        protected IWebElement FindSingleElementByXPath(string xpath)
        {
            return driver.FindElement(By.XPath(xpath), timeoutInSeconds);
        }

        protected IWebElement FindSingleElementByCssSelector(string cssSelector)
        {
            return driver.FindElement(By.CssSelector(cssSelector), timeoutInSeconds);
        }

        protected ReadOnlyCollection<IWebElement> FindElementsByXPath(string xpath)
        {
            return driver.FindElements(By.XPath(xpath));
        }

        protected IWebElement FindSingleElementByName(string name)
        {
            return driver.FindElement(By.Name(name), timeoutInSeconds);
        }

        protected IWebElement FindSingleElementByClass(string cssClass)
        {
            return driver.FindElement(By.ClassName(cssClass), timeoutInSeconds);
        }

        protected ReadOnlyCollection<IWebElement> FindElementsByClass(string cssClass)
        {
            return driver.FindElements(By.ClassName(cssClass));
        }

        protected bool IsElementVisible(string id)
        {
            try
            {
                var element = driver.FindElement(By.Id(id));
                return element.Displayed && element.Enabled;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        void AssertSoon(Func<bool> func, string message = "", int maxSeconds = 5)
        {
            var stopwatch = Stopwatch.StartNew();
            var conditionMet = func();
            while (stopwatch.ElapsedMilliseconds < maxSeconds * 1000 && !conditionMet)
            {
                Thread.Sleep(100);
                conditionMet = func();
            }
            Assert.IsTrue(conditionMet, message);
        }
    }
}