namespace bootcamp_api.Tests.NUnit.Services;

[TestFixture]
public class UserServiceTests
{
	private IUserService _userService;
	private IMapper _mapper;
	private DbConnection _connection;
	private DbContextOptions<PawssierContext> _contextOptions;
	private ApiVersion apiVersion;
	private User[] dbUsers;

	public Dto.User NewUser()
	{
		return new Dto.User
		{
			FirstName = "Pawssier",
			LastName = "User",
			Email = "new@pawssier.com",
			Birthday = new DateTime(),
			Password = "Password123"
		};
	}

	private Dto.User MapUser(Domain.User domainUser)
	{
		return new Dto.User
		{
			FirstName = domainUser.FirstName,
			LastName = domainUser.LastName,
			Email = domainUser.Email,
			Birthday = domainUser.Birthday,
			Password = domainUser.Password
		};
	}

	[OneTimeSetUp]
	public void Init()
	{
		dbUsers = new User[]
		{
			new Domain.User
			{
				FirstName = "Pawssier",
				LastName = "User1",
				Email = "1@pawssier.com",
				Birthday = new DateTime(),
				Password = "Password123",
				DateAdded = new DateTime(),
				DateModified = new DateTime()
			},
			new Domain.User
			{
				FirstName = "Pawssier",
				LastName = "User2",
				Email = "2@pawssier.com",
				Birthday = new DateTime(),
				Password = "Password123",
				DateAdded = new DateTime(),
				DateModified = new DateTime()
			}
		};

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
                FROM Users;";
			viewCommand.ExecuteNonQuery();
		}

		context.Users.AddRange(dbUsers);

		context.SaveChanges();

		_userService = new UserService(context, _mapper);
	}

	[TearDown]
	public void CleanUp()
	{
		_connection.Dispose();
	}

	[Test]
	public void Authenticate_Throws_UnauthorizedException_When_Password_Does_Not_Pair_With_Email()
	{
		User user = dbUsers[0];
		Dto.Credentials creds = new Dto.Credentials
		{
			Email = user.Email,
			Password = "NoMatch"
		};

		var act = () => _userService.Authenticate(apiVersion, creds);

		act.Should().Throw<UnauthorizedException>();
	}

	[Test]
	public void Authenticate_Throws_UnauthorizedException_When_No_Users_With_Email_Exist()
	{
		Dto.Credentials creds = new Dto.Credentials
		{
			Email = "email@somewhere.com",
			Password = "NoMatch"
		};

		var act = () => _userService.Authenticate(apiVersion, creds);

		act.Should().Throw<UnauthorizedException>();
	}

	[Test]
	public void Authenticate_Calls_LinkService_GenerateUserLink()
	{
		User user = dbUsers[0];
		Dto.Credentials creds = new Dto.Credentials
		{
			Email = user.Email,
			Password = user.Password
		};

		var res = _userService.Authenticate(apiVersion, creds);

		res.Link.Should().NotBeNullOrWhiteSpace();
	}

	[Test]
	public void Add_Throws_DuplicateUserException_If_Email_Is_Already_In_Use()
	{
		Dto.User user = MapUser(dbUsers[0]);

		var act = () => _userService.Add(apiVersion, user);

		act.Should().Throw<DuplicateUserException>();
	}

	[Test]
	public void Add_Calls_LinkService_Generate_User_Link()
	{
		Dto.User user = NewUser();

		var res = _userService.Add(apiVersion, user);

		res.Link.Should().NotBeNullOrWhiteSpace();
	}

	[Test]
	public void Delete_Throws_UserNotFoundException_If_User_Does_Not_Exist()
	{
		var act = () => _userService.Delete(-1);

		act.Should().Throw<UserNotFoundException>();
	}

	[Test]
	public void Update_Throws_Exception_If_User_Id_Does_Not_Match_Parameter_Id()
	{
		Dto.User user = NewUser();
		user.Id = -1;

		var act = () => _userService.Update(apiVersion, 1, user);

		act.Should().Throw<Exception>();
	}

	[Test]
	public void Update_Throws_UserNotFoundException_If_User_Does_Not_Exist()
	{
		Dto.User user = NewUser();
		user.Id = -1;

		var act = () => _userService.Update(apiVersion, user.Id, user);

		act.Should().Throw<UserNotFoundException>();
	}

	[Test]
	public void Update_Throws_DuplicateUserException_If_Updated_Email_Is_Already_In_Use()
	{
		Dto.User user = NewUser();
		user.Id = 2;
		user.Email = dbUsers[0].Email;

		var act = () => _userService.Update(apiVersion, user.Id, user);

		act.Should().Throw<DuplicateUserException>();
	}

	[Test]
	public void Update_Calls_LinkService_Generate_User_Link()
	{
		Dto.User user = NewUser();
		user.Id = 1;
		user.Email = "unique@gmail.com";

		var res = _userService.Update(apiVersion, user.Id, user);

		res.Link.Should().NotBeNullOrWhiteSpace();
	}
}