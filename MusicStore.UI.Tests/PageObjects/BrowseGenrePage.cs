using System.Collections.ObjectModel;
using NUnit.Framework;
using OpenQA.Selenium;

namespace MusicStore.UI.Tests.PageObjects
{
    public class BrowseGenrePage : PageObject
    {
        public BrowseGenrePage(IWebDriver driver)
            : base(driver)
        {
        }

        public void AssertThatAlbumExists(string albumTitle)
        {
            Assert.NotNull(AlbumLink(albumTitle));
        }

        ReadOnlyCollection<IWebElement> AlbumLink(string albumTitle)
        {
            return FindElementsByXPath("//a/span[contains(., '" + albumTitle + "')]");
        }
    }
}