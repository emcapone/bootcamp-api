namespace bootcamp_api.Tests.Services;

public class BookmarkServiceTests: IDisposable
{

    private readonly DbConnection _connection;
    private readonly DbContextOptions<PawssierContext> _contextOptions;

    public BookmarkServiceTests()
    {
        // Create and open a connection. This creates the SQLite in-memory database, which will persist until the connection is closed
        // at the end of the test (see Dispose below).
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();

        // These options will be used by the context instances in this test suite, including the connection opened above.
        _contextOptions = new DbContextOptionsBuilder<PawssierContext>()
            .UseSqlite(_connection)
            .Options;

        // Create the schema and seed some data
        using var context = new PawssierContext(_contextOptions);

        if (context.Database.EnsureCreated())
        {
            using var viewCommand = context.Database.GetDbConnection().CreateCommand();
            viewCommand.CommandText = @"
                CREATE VIEW AllResources AS
                SELECT Id
                FROM Bookmarks;";
            viewCommand.ExecuteNonQuery();
        }

        context.Bookmarks.AddRange(
            new Bookmark
            {
                Id = 1,
                Link = "\\v1\\pet\\12345",
                Petfinder_Id = 12345,
                Title = "Bookmark 1 Title",
                Note = "Bookmark created on Safari",
                SavedAt = DateTime.UtcNow,
                External_Url = "petfinder.com/12345",
                DateModified = DateTime.UtcNow
            },
            new Bookmark
            {
                Id = 2,
                Link = "\\v1\\pet\\67890",
                Petfinder_Id = 67890,
                Title = "Bookmark 2 Title",
                Note = "Bookmark created on Safari",
                SavedAt = DateTime.UtcNow,
                External_Url = "petfinder.com/67890",
                DateModified = DateTime.UtcNow
            });
        context.SaveChanges();
    }

    PawssierContext CreateContext() => new PawssierContext(_contextOptions);

    public void Dispose() => _connection.Dispose();
    
    [Fact]
    public async void GetAll_Returns_All_Bookmarks()
    {
        //Arrange
        var context = CreateContext();
        var bookmarkService = new BookmarkService(context);
        var currentCount = context.Bookmarks.Count();
        //Act
        var bookmarks = bookmarkService.GetAll();
        //Assert
        bookmarks.Length.Should().Be(currentCount);
    }

    [Fact]
    public void Get_Returns_Correct_Bookmark()
    {
        //Arrange
        var context = CreateContext();
        var bookmarkService = new BookmarkService(context);
        //Act
        var bookmark = bookmarkService.Get(1);
        //Assert
        bookmark.Id.Should().Be(1);
    }

    [Fact]
    public void Get_Throws_BookmarkNotFoundException_When_Bookmark_Does_Not_Exist()
    {
        //Arrange
        var context = CreateContext();
        var bookmarkService = new BookmarkService(context);
        //Act
        Action act = () => bookmarkService.Get(-1);
        //Assert
        act.Should().Throw<BookmarkNotFoundException>();
    }

    [Fact]
    public void Add_Creates_A_Bookmark()
    {
        //Arrange
        var newBookmark = new Dto.Bookmark
            {
                Link = "\\v1\\pet\\99",
                Petfinder_Id = 99,
                Title = "Bookmark 3 Title",
                Note = "Bookmark created on Safari",
                SavedAt = DateTime.UtcNow,
                External_Url = "petfinder.com/99"
            };
        var context = CreateContext();
        var bookmarkService = new BookmarkService(context);
        var currentCount = context.Bookmarks.Count();
        //Act
        bookmarkService.Add(newBookmark);
        //Assert
        context.Bookmarks.Count().Should().Be(currentCount + 1);
    }

    [Fact]
    public void Add_Throws_DuplicateBookmarkException_When_Petfinder_Id_Is_Not_Unique()
    {
        //Arrange
        var context = CreateContext();
        var bookmarkService = new BookmarkService(context);
        var duplicatePetfinderId = context.Bookmarks.SingleOrDefault(b => b.Id == 1).Petfinder_Id;
        var newBookmark = new Dto.Bookmark
        {
            Link = "\\v1\\pet\\99",
            Petfinder_Id = duplicatePetfinderId,
            Title = "Bookmark 3 Title",
            Note = "Bookmark created on Safari",
            SavedAt = DateTime.UtcNow,
            External_Url = "petfinder.com/99"
        };
        //Act
        Action act = () => bookmarkService.Add(newBookmark);
        //Assert
        act.Should().Throw<DuplicateBookmarkException>();
    }

    [Fact]
    public void Delete_Removes_A_Bookmark()
    {
        //Arrange
        var context = CreateContext();
        var bookmarkService = new BookmarkService(context);
        int id = 1;
        //Act
        bookmarkService.Delete(1);
        //Assert
        context.Bookmarks.SingleOrDefault(b => b.Id == id).Should().BeNull();
    }

    [Fact]
    public void Delete_Throws_BookmarkNotFoundException_When_Bookmark_Does_Not_Exist()
    {
        //Arrange
        var context = CreateContext();
        var bookmarkService = new BookmarkService(context);
        //Act
        Action act = () => bookmarkService.Delete(-1);
        //Assert
        act.Should().Throw<BookmarkNotFoundException>();
    }

    [Fact]
    public void Update_Updates_Correct_Bookmark()
    {
        //Arrange
        var updatedBookmark = new Dto.Bookmark
        {
            Id = 2,
            Link = "\\v1\\pet\\67890",
            Petfinder_Id = 67890,
            Title = "Bookmark 2 Title: Revamped",
            Note = "Bookmark created on Safari",
            SavedAt = DateTime.UtcNow,
            External_Url = "petfinder.com/67890"
        };
        var context = CreateContext();
        var bookmarkService = new BookmarkService(context);
        //Act
        var bookmark = bookmarkService.Update(2, updatedBookmark);
        //Assert
        bookmark.Title.Should().Be(updatedBookmark.Title);
    }

    [Fact]
    public void Update_Throws_BookmarkNotFoundException_When_Bookmark_Does_Not_Exist()
    {
        //Arrange
        var updatedBookmark = new Dto.Bookmark
        {
            Id = -1,
            Link = "\\v1\\pet\\67890",
            Petfinder_Id = 67890,
            Title = "Bookmark -1 Title: Revamped",
            Note = "Bookmark created on Safari",
            SavedAt = DateTime.UtcNow,
            External_Url = "petfinder.com/67890"
        };
        var context = CreateContext();
        var bookmarkService = new BookmarkService(context);
        //Act
        Action act = () => bookmarkService.Update(-1, updatedBookmark);
        //Assert
        act.Should().Throw<BookmarkNotFoundException>();
    }

    [Fact]
    public void Update_Throws_Exception_When_Bookmark_Id_Does_Not_Match_Parameter_Id()
    {
        //Arrange
        var updatedBookmark = new Dto.Bookmark
        {
            Id = 3,
            Link = "\\v1\\pet\\67890",
            Petfinder_Id = 67890,
            Title = "Bookmark -1 Title: Revamped",
            Note = "Bookmark created on Safari",
            SavedAt = DateTime.UtcNow,
            External_Url = "petfinder.com/67890"
        };
        var context = CreateContext();
        var bookmarkService = new BookmarkService(context);
        //Act
        Action act = () => bookmarkService.Update(4, updatedBookmark);
        //Assert
        act.Should().Throw<Exception>();
    }
}