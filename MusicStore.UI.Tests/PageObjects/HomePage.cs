using System.Linq;
using NUnit.Framework;
using OpenQA.Selenium;

namespace MusicStore.UI.Tests.PageObjects
{
    public class HomePage : PageObject
    {
        public HomePage(IWebDriver driver)
            : base(driver)
        {
        }

        public void AssertThatGenreExists(string genre)
        {
            Assert.NotNull(GenreLink(genre));
        }

        public BrowseGenrePage NavigateToGenre(string genre)
        {
            var genreLink = GenreLink(genre);
            genreLink.Click();
            return new BrowseGenrePage(driver);
        }

        IWebElement GenreLink(string genre)
        {
            return FindElementsByXPath("//a[contains(., '" + genre + "')]").FirstOrDefault();
        }
    }
}