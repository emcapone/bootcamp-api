namespace bootcamp_api.Tests.XUnit.Services;

public class BookmarkServiceTests: IDisposable
{

    private readonly DbConnection _connection;
    private readonly DbContextOptions<PawssierContext> _contextOptions;
    private readonly IMapper _mapper;
    private readonly int user_id = 1;
    private readonly ApiVersion apiVersion = new ApiVersion(1, 0);
    private readonly int pfVersion = 2;

    public BookmarkServiceTests()
    {
        if (_mapper == null)
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new PawssierMappingProfile());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            _mapper = mapper;
        }

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

        context.Users.Add(
            new User
            {
                Id = user_id,
                FirstName = "Pawssier",
                LastName = "User",
                Email = "user@pawssier.com",
                Birthday = new DateTime(),
                Password = "Password123"
            });

        context.Bookmarks.AddRange(
            new Bookmark
            {
                Id = 1,
                Petfinder_id = 12345,
                Title = "Bookmark 1 Title",
                Note = "Bookmark created on Safari",
                SavedAt = DateTime.UtcNow,
                External_url = "petfinder.com/12345",
                DateModified = DateTime.UtcNow,
                User_id = user_id
            },
            new Bookmark
            {
                Id = 2,
                Petfinder_id = 67890,
                Title = "Bookmark 2 Title",
                Note = "Bookmark created on Safari",
                SavedAt = DateTime.UtcNow,
                External_url = "petfinder.com/67890",
                DateModified = DateTime.UtcNow,
                User_id = user_id
            });
        context.SaveChanges();
    }

    PawssierContext CreateContext() => new PawssierContext(_contextOptions);

    public void Dispose() => _connection.Dispose();
    
    [Fact]
    public void GetAll_Returns_All_Of_A_Users_Bookmarks()
    {
        //Arrange
        var context = CreateContext();
        var bookmarkService = new BookmarkService(context, _mapper);
        var currentCount = context.Bookmarks.Where(b => b.User_id == user_id).Count();
        //Act
        var bookmarks = bookmarkService.GetAll(apiVersion, user_id, pfVersion);
        //Assert
        bookmarks.Length.Should().Be(currentCount);
    }

    [Fact]
    public void Get_Returns_Correct_Bookmark()
    {
        //Arrange
        var context = CreateContext();
        var bookmarkService = new BookmarkService(context, _mapper);
        var id = 1;
        //Act
        var bookmark = bookmarkService.Get(apiVersion, id, pfVersion);
        //Assert
        bookmark.Id.Should().Be(id);
    }

    [Fact]
    public void Get_Throws_BookmarkNotFoundException_When_Bookmark_Does_Not_Exist()
    {
        //Arrange
        var context = CreateContext();
        var bookmarkService = new BookmarkService(context, _mapper);
        //Act
        Action act = () => bookmarkService.Get(apiVersion, -1, pfVersion);
        //Assert
        act.Should().Throw<BookmarkNotFoundException>();
    }

    [Fact]
    public void Add_Creates_A_Bookmark()
    {
        //Arrange
        var newBookmark = new Dto.Bookmark
            {
                Petfinder_id = 99,
                Title = "Bookmark 3 Title",
                Note = "Bookmark created on Safari",
                SavedAt = DateTime.UtcNow,
                External_url = "petfinder.com/99"
            };
        var context = CreateContext();
        var bookmarkService = new BookmarkService(context, _mapper);
        var currentCount = context.Bookmarks.Where(b => b.User_id == user_id).Count();
        //Act
        bookmarkService.Add(apiVersion, user_id, pfVersion, newBookmark);
        //Assert
        context.Bookmarks.Count().Should().Be(currentCount + 1);
    }

    [Fact]
    public void Add_Throws_DuplicateBookmarkException_When_Petfinder_Id_Is_Not_Unique()
    {
        //Arrange
        var context = CreateContext();
        var bookmarkService = new BookmarkService(context, _mapper);
        var duplicatePetfinderId = context.Bookmarks.SingleOrDefault(b => b.Id == 1).Petfinder_id;
        var newBookmark = new Dto.Bookmark
        {
            Petfinder_id = duplicatePetfinderId,
            Title = "Bookmark 3 Title",
            Note = "Bookmark created on Safari",
            SavedAt = DateTime.UtcNow,
            External_url = "petfinder.com/99"
        };
        //Act
        Action act = () => bookmarkService.Add(apiVersion, user_id, pfVersion, newBookmark);
        //Assert
        act.Should().Throw<DuplicateBookmarkException>();
    }

    [Fact]
    public void Delete_Removes_A_Bookmark()
    {
        //Arrange
        var context = CreateContext();
        var bookmarkService = new BookmarkService(context, _mapper);
        int id = 1;
        //Act
        bookmarkService.Delete(id);
        //Assert
        context.Bookmarks.SingleOrDefault(b => b.Id == id).Should().BeNull();
    }

    [Fact]
    public void Delete_Throws_BookmarkNotFoundException_When_Bookmark_Does_Not_Exist()
    {
        //Arrange
        var context = CreateContext();
        var bookmarkService = new BookmarkService(context, _mapper);
        //Act
        Action act = () => bookmarkService.Delete(-1);
        //Assert
        act.Should().Throw<BookmarkNotFoundException>();
    }

    [Fact]
    public void Update_Updates_Correct_Bookmark()
    {
        //Arrange
        var id = 2;
        var updatedBookmark = new Dto.Bookmark
        {
            Id = id,
            Petfinder_id = 67890,
            Title = "Bookmark 2 Title: Revamped",
            Note = "Bookmark created on Safari",
            SavedAt = DateTime.UtcNow,
            External_url = "petfinder.com/67890"
        };
        var context = CreateContext();
        var bookmarkService = new BookmarkService(context, _mapper);
        //Act
        var bookmark = bookmarkService.Update(apiVersion, id, pfVersion, updatedBookmark);
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
            Petfinder_id = 67890,
            Title = "Bookmark -1 Title: Revamped",
            Note = "Bookmark created on Safari",
            SavedAt = DateTime.UtcNow,
            External_url = "petfinder.com/67890"
        };
        var context = CreateContext();
        var bookmarkService = new BookmarkService(context, _mapper);
        //Act
        Action act = () => bookmarkService.Update(apiVersion, -1, pfVersion, updatedBookmark);
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
            Petfinder_id = 67890,
            Title = "Bookmark -1 Title: Revamped",
            Note = "Bookmark created on Safari",
            SavedAt = DateTime.UtcNow,
            External_url = "petfinder.com/67890"
        };
        var context = CreateContext();
        var bookmarkService = new BookmarkService(context, _mapper);
        //Act
        Action act = () => bookmarkService.Update(apiVersion, 4, pfVersion, updatedBookmark);
        //Assert
        act.Should().Throw<Exception>();
    }
}