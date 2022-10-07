namespace bootcamp_api.Tests.NUnit.Services;

[TestFixture]
public class CalendarEventServiceTests
{
    private ICalendarEventService _eventService;
    private IMapper _mapper;
    private DbConnection _connection;
    private DbContextOptions<PawssierContext> _contextOptions;
    private int user_id;
    private ApiVersion apiVersion;

    public Dto.CalendarEvent NewCalendarEvent()
    {
        return new Dto.CalendarEvent
        {
            Date = new DateTime(),
            AllDay = false,
            StartTime = "17:00",
            EndTime = "18:00",
            Name = "Fun Event",
            Details = "Definitely fun"
        };
    }

    public Domain.CalendarEvent NewDomainCalendarEvent()
    {
        return new Domain.CalendarEvent
        {
            Date = new DateTime(),
            AllDay = false,
            StartTime = "17:00",
            EndTime = "18:00",
            Name = "Fun Event",
            Details = "Definitely fun",
            DateAdded = new DateTime(),
            DateModified = new DateTime(),
            User_id = user_id
        };
    }

    [OneTimeSetUp]
    public void Init()
    {
        user_id = 1;
        apiVersion = new ApiVersion(1, 0);

        var mappingConfig = new MapperConfiguration(mc =>
        {
            mc.AddProfile(new PawssierMappingProfile());
        });
        IMapper mapper = mappingConfig.CreateMapper();
        _mapper = mapper;
    }

    [SetUp]
    public void SetUp()
    {
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();

        _contextOptions = new DbContextOptionsBuilder<PawssierContext>()
            .UseSqlite(_connection)
            .Options;

        var context = new PawssierContext(_contextOptions);

        if (context.Database.EnsureCreated())
        {
            using var viewCommand = context.Database.GetDbConnection().CreateCommand();
            viewCommand.CommandText = @"
                CREATE VIEW AllResources AS
                SELECT Id
                FROM CalendarEvents;";
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

        context.CalendarEvents.Add(NewDomainCalendarEvent());
        context.SaveChanges();

        _eventService = new CalendarEventService(context, _mapper);
    }

    [TearDown]
    public void CleanUp()
    {
        _connection.Dispose();
    }

    [Test]
    public void Get_Throws_Exception_When_Month_Is_Invalid()
    {
        var act = () => _eventService.Get(apiVersion, user_id, 14, 2022);

        act.Should().Throw<Exception>();
    }

    [Test]
    public void Get_Throws_Exception_When_Year_Is_Invalid()
    {
        var act = () => _eventService.Get(apiVersion, user_id, 12, -1989);

        act.Should().Throw<Exception>();
    }

    [Test]
    public void Get_Throws_Exception_When_Only_Month_Is_Provided()
    {
        var act = () => _eventService.Get(apiVersion, user_id, 12);

        act.Should().Throw<Exception>();
    }

    [Test]
    public void Get_Returns_Dto_CalendarEvent_Array_If_No_Month_Or_Year_Is_Passed()
    {
        var res = _eventService.Get(apiVersion, user_id);

        res.Should().BeOfType<Dto.CalendarEvent[]>();
    }

    [Test]
    public void Get_Returns_CalendarEvent_Array_If_A_Year_Is_Passed()
    {
        var res = _eventService.Get(apiVersion, user_id, 0, 2022);

        res.Should().BeOfType<Dto.CalendarEvent[]>();
    }

    [Test]
    public void Get_Returns_CalendarEvent_Array_If_A_Month_And_Year_Is_Passed()
    {
        var res = _eventService.Get(apiVersion, user_id, year:2022);

        res.Should().BeOfType<Dto.CalendarEvent[]>();
    }

    [Test]
    public void Get_Calls_LinkService_GenerateCalendarEventsLink_For_Each_CalendarEvent()
    {
        Dto.CalendarEvent[] res = _eventService.Get(apiVersion, user_id);

        foreach (Dto.CalendarEvent x in res)
        {
            x.Link.Should().NotBeNullOrWhiteSpace();
        }
    }

    [Test]
    public void GetById_Throws_CalendarEventNotFoundException_If_CalendarEvent_Does_Not_Exist()
    {
        var act = () => _eventService.GetById(apiVersion, -1);

        act.Should().Throw<CalendarEventNotFoundException>();
    }

    [Test]
    public void GetById_Calls_LinkService_GenerateCalendarEventsLink()
    {
        var res = _eventService.GetById(apiVersion, 1);

        res.Link.Should().NotBeNullOrWhiteSpace();
    }

    [Test]
    public void Add_Throws_DuplicateCalendarEventException_If_CalendarEvent_Already_Exists()
    {
        var act = () => _eventService.Add(apiVersion, user_id, NewCalendarEvent());

        act.Should().Throw<DuplicateCalendarEventException>();
    }

    [Test]
    public void Add_Calls_LinkService_GenerateCalendarEventsLink()
    {
        var newEvent = NewCalendarEvent();
        newEvent.Name = "Brand New Event";

        var res = _eventService.Add(apiVersion, user_id, newEvent);

        res.Link.Should().NotBeNullOrWhiteSpace();
    }

    [Test]
    public void Delete_Throws_CalendarEventNotFoundException_If_CalendarEvent_Does_Not_Exist()
    {
        var act = () => _eventService.Delete(-1);

        act.Should().Throw<CalendarEventNotFoundException>();
    }

    [Test]
    public void Update_Throws_Exception_If_CalendarEvent_Id_Does_Not_Match_Parameter_Id()
    {
        var updatedEvent = NewCalendarEvent();
        updatedEvent.Id = 2;

        var act = () => _eventService.Update(apiVersion, 1, updatedEvent);

        act.Should().Throw<Exception>();
    }

    [Test]
    public void Update_Throws_CalendarEventNotFoundException_If_CalendarEvent_Does_Not_Exist()
    {
        var updatedEvent = NewCalendarEvent();
        updatedEvent.Id = -1;

        var act = () => _eventService.Update(apiVersion, -1, updatedEvent);

        act.Should().Throw<CalendarEventNotFoundException>();
    }

    [Test]
    public void Update_Calls_LinkService_GenerateCalendarEventsLink()
    {
        var updatedEvent = NewCalendarEvent();
        updatedEvent.Id = 1;

        var res = _eventService.Update(apiVersion, 1, updatedEvent);

        res.Link.Should().NotBeNullOrWhiteSpace();
    }
}