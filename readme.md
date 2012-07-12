The MVC Music Store is a pretty straight forward example of an MVC 3 / EF 4 e-Commerce site.
Thanks to Jon Galloway for all the hard work he put into creating it. You can find his original source and instructions for building the application here http://mvcmusicstore.codeplex.com/

After I got the application working on my local machine, I changed it to use IIS rather than Casini because Casini sucks and should never ever be used by real developers.

The Store Controller seems pretty straight forward, let's create a test for it.
Before we start, we need to checkout the correct commit which is 110890a IIS > Casini. We also need to add a hosts file entry for musicstore.local.

Now that we are ready, we need a test project, so let's create a class library.
Then we need a test framework, so we use nuget to install-package nunit.
And while we are in the nuget console, we will grab a mocking library as well. install-package moq.

Now we can create a StoreControllerTests class
```cs
[TestFixture]
public class StoreControllerTests
{

}
```

Our first test is on the index: 
```cs
[Test]
public void Index_displays_the_list_of_genres()
{
	//Arrange
	StoreController storeController = new StoreController();
	MusicStoreEntities storeDb = new MusicStoreEntities();
	//Act
	ViewResult indexResult = (ViewResult) storeController.Index();
	//Assert
	Assert.AreEqual(storeDb.Genres, indexResult.Model);
}
```
This test was pretty simple, but it isn't a very good test. I can make it pass with the wrong code:
```cs
public ActionResult Index()
{
	return View(new Genre[]{});
}
```
It also takes a long time to complete.
So the test isn't very good, but it's a start. 
Let's move on for now and test the Details method:
```cs
[Test]
public void Details_shows_one_album_by_id()
{
	StoreController storeController = new StoreController();
	MusicStoreEntities storeDb = new MusicStoreEntities();

	ViewResult detailsResult = (ViewResult)storeController.Details(5);

	Assert.AreEqual(storeDb.Albums.Find(5), detailsResult.Model);
}
```
That test isn't any better than the first, but I want to write one more and see if that leads me to any other strategies:
```cs
[Test]
public void Browse_displays_the_albums_in_a_genre()
{
	StoreController storeController = new StoreController();
	MusicStoreEntities storeDb = new MusicStoreEntities();

	ViewResult browseResult = (ViewResult)storeController.Browse("Rock");

	Genre genre = storeDb.Genres.Include("Albums").Single(g => g.Name == "Rock");
	Assert.AreEqual(genre, browseResult.Model);
}
```
This test doesn't even pass because the test AND the code require that Genres return exactly one matching result and they return no results.

Now we have 3 tests that take 15-20 seconds each to run and don't do a very good job ensuring our functionality is correct.
Obviously, this isn't the direction we want to go. What can we do?

Well the first thing is to loosen the coupling to the database. The controller uses the MusicStoreEntities class directly with is coupled to the database.

We can extract an interface from the MusicStoreEntities DbContext directly, so let's see where that goes.
We don't need setters for these properties, so let's remove those.
Now we can mock the call to the DbContext and modify our test like so:
```cs
[Test]
public void Browse_displays_the_albums_in_a_genre()
{
	var musicStoreEntities = new Mock<IMusicStoreEntities>();
	StoreController storeController = new StoreController(musicStoreEntities.Object);
	var genres = new DbSet<Genre>();
	musicStoreEntities.Setup(x => x.Genres).Returns(genres);

	ViewResult browseResult = (ViewResult) storeController.Browse("Rock");

	Assert.AreEqual(genres, browseResult.Model);
}
```
The unfortunate thing is that DbSet<T> has an internal ctor and no derived types, so we need a different strategy.
I think it is time to introduce a new seam into our application. We will borrow a term from Domain Driven Design and start implementmenting an Application Layer.
For now, we will keep the application layer services in the same project.
I will create an Application folder and add to it a new interface: IStoreService. This isn't a great name, but it is the first thing we are putting in this layer, so we will use a simple name for now and refactor as we learn more.
```cs
public interface IStoreService
{
	Genre FindGenreByName(string genre);
}
```
```cs
public class StoreService : IStoreService
{
	private readonly MusicStoreEntities musicStoreEntities;

	public StoreService()
	{
		musicStoreEntities = new MusicStoreEntities();
	}

	public Genre FindGenreByName(string genre)
	{
		return musicStoreEntities
			.Genres
			.Include("Albums")
			.Single(g => g.Name == genre);
	}
}
```
After we fix up our controller's ctor:
```cs
private readonly IStoreService storeService;
private MusicStoreEntities storeDb;

public StoreController()
	: this(new StoreService())
{
}

public StoreController(IStoreService storeService)
{
	this.storeService = storeService;
	this.storeDb = new MusicStoreEntities();
}
```
We can update our test like so:
```cs
[Test]
public void Browse_displays_the_albums_in_a_genre()
{
	var musicStoreService = new Mock<IStoreService>();
	StoreController storeController = new StoreController(musicStoreService.Object);
	var genre = new Genre();
	musicStoreService.Setup(x => x.FindGenreByName(It.IsAny<string>())).Returns(genre);

	ViewResult browseResult = (ViewResult)storeController.Browse("Rock");

	Assert.AreEqual(genre, browseResult.Model);
}
```
And the Browse action becomes:
```cs
public ActionResult Browse(string genre)
{
	Genre genreModel = storeService.GetGenreByName(genre);
	return View(genreModel);
}
```
So what does this do? Well, let's run the test. Now it takes less than 2 seconds on this really slow old laptop, including the start up time for the test framework.

So now we need to correct the other 2 tests.
First, update the test:
```cs
[Test]
public void Details_shows_one_album_by_id()
{
	var musicStoreService = new Mock<IStoreService>();
	StoreController storeController = new StoreController(musicStoreService.Object);
	var albumFromService = new Album();
	musicStoreService.Setup(x => x.GetAlbumById(It.IsAny<int>())).Returns(albumFromService);

	ViewResult detailsResult = (ViewResult)storeController.Details(5);

	Assert.AreEqual(albumFromService, detailsResult.Model);
}
```
Update the interface:
```cs
public interface IStoreService
{
	Genre FindGenreByName(string genre);
	Album GetAlbumById(int albumId);
}
```
Make it pass:
```cs
public ActionResult Details(int id)
{
	Album album = storeService.GetAlbumById(id);
	return View(album);
}
```
Move the extisting implementation to the StoreService class:
```cs
public Album GetAlbumById(int albumId)
{
	return musicStoreEntities.Albums.Find(albumId);
}
```
For Index, the process is similar:
First, update the test:
```cs
[Test]
public void Index_displays_the_list_of_genres()
{
	//Arrange
	var musicStoreService = new Mock<IStoreService>();
	StoreController storeController = new StoreController(musicStoreService.Object);
	var genres = new []{new Genre()};
	musicStoreService.Setup(x => x.GetAllGenres()).Returns(genres);
	//Act
	ViewResult indexResult = (ViewResult)storeController.Index();
	//Assert
	Assert.AreEqual(genres, indexResult.Model);
}
```
Make it pass:
```cs
public ActionResult Index()
{
	var genres = storeService.GetAllGenres();
	return View(genres);
}
```
Move the extisting implementation to the StoreService class:
```cs
public IEnumerable<Genre> GetAllGenres()
{
	return musicStoreEntities.Genres.ToList();
}
```
Now we have 3 tests that pass in less than 2 seconds rather than 60 seconds.
Ok, now let's clean up the duplication in StoreControllerTests before we move on.
Add a SetUp method:
```cs
private Mock<IStoreService> musicStoreService;
private StoreController storeController;

[SetUp]
public void SetUp()
{
	musicStoreService = new Mock<IStoreService>();
	storeController = new StoreController(musicStoreService.Object);
}
```
Then remove the redundent setup from the tests themselves. (Make sure you run them after!)

With that done, let's test the last method in the controller before me move on.
```cs
[Test]
public void GenreMenu_displays_the_list_of_genres()
{
	Genre[] genres = new[] { new Genre() };
	musicStoreService.Setup(x => x.GetAllGenres()).Returns(genres);

	PartialViewResult menuResult = (PartialViewResult)storeController.GenreMenu();

	Assert.AreEqual(genres, menuResult.Model);
}
```
Make it pass:
```cs
[ChildActionOnly]
public ActionResult GenreMenu()
{
	var genres = storeService.GetAllGenres();
	return PartialView(genres);
}
```
So now let's clean up the controller by removing the unreferenced code and the pointless comments.

At this point, I would like to move the Application, Domain, Infrastructure, and ViewModel components of 
the solution into a separate project, so let's take a look at what would happen if we do that right now.
Check out the Application branch to see what this looks like.

So we see that there are some problems at this point:
- Both Album and Order are coupled to the System.Web.Mvc.BindAttribute
- Both ChangePasswordModel and RegisterModel are coupled to System.Web.Mvc.CompareAttribute
- ShoppingCart is coupled to HttpContext, HttpContextBase, and Controller
 
How do we address these issues?
- Rather than using entities in our Views and Controllers, we can use ViewModels this will resolve the BindAttribute
- The HttpContext and HttpContextBase can easily be put behind a service with an interface defined in the MusicStore. This would also eliminate the coupling to Controller

The only really tricky issue is using the CompareAttribute. 
