using NUnit.Framework;

namespace MusicStore.UI.Tests.Tests
{
    public class CatalogNavigationTests : UiTest
    {
        [Test]
        public void Verify_genre_existence()
        {
            var homePage = navigateTo.HomePage();
            homePage.AssertThatGenreExists("Rock");
        } 

        [Test]
        public void Verify_that_user_can_browse_genre()
        {
            var homePage = navigateTo.HomePage();
            var browseGenrePage = homePage.NavigateToGenre("Rock");
            browseGenrePage.AssertThatAlbumExists("Jagged Little Pill");
        } 
    }
}