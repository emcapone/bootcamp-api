namespace bootcamp_api.Tests.NUnit.Services;

[TestFixture]
public class MessageServiceTests
{
    private IMessageService _messageService;
    private IMapper _mapper;
    private DbConnection _connection;
    private DbContextOptions<PawssierContext> _contextOptions;

    public Dto.Message newMessage()
    {
        return new Dto.Message
        {
            Email = "user@gmail.com",
            Category = "general",
            Subject = "New Message",
            Body = "Some message"
        };
    }

    [OneTimeSetUp]
    public void Init()
    {
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
                FROM Messages;";
            viewCommand.ExecuteNonQuery();
        }

        context.SaveChanges();

        _messageService = new MessageService(context, _mapper);
    }

    [TearDown]
    public void CleanUp()
    {
        _connection.Dispose();
    }
    
    [Test]
    public void Add_Returns_A_Dto_Message()
    {
        Dto.Message message = newMessage();

        var returnedMessage = _messageService.Add(message);

        returnedMessage.Should().BeOfType<Dto.Message>();
    }

}