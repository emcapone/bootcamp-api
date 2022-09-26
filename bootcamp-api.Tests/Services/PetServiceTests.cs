namespace bootcamp_api.Tests.Services;

public class PetServiceTests: IDisposable
{

    private readonly DbConnection _connection;
    private readonly DbContextOptions<PawssierContext> _contextOptions;
    private readonly IMapper _mapper;
    private readonly ApiVersion apiVersion = new ApiVersion(1, 0);
    private readonly int user_id = 1;

    public PetServiceTests()
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
                FROM Pets;";
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

        context.Pets.AddRange( NewDomainPet(), NewDomainPet() );

        context.SaveChanges();
    }

    PawssierContext CreateContext() => new PawssierContext(_contextOptions);

    public void Dispose() => _connection.Dispose();

    public Pet NewDomainPet()
    {
        List<Vaccine> vaccines = new List<Vaccine>();
        vaccines.Add(new Vaccine
                    {
                        Name = "string",
                        DateAdministered = DateTime.UtcNow,
                        DueDate = DateTime.UtcNow
                    });
        List<Condition> conditions = new List<Condition>();
        conditions.Add(new Condition
                    {
                        Name = "string",
                        Notes = "string"
                    });
        List<Prescription> prescriptions = new List<Prescription>();
        prescriptions.Add(new Prescription
                    {
                        Name = "string",
                        Doctor = "string",
                        Due = DateTime.UtcNow,
                        Refills = 0
                    });
        return new Pet
            {
                Name = "string",
                Breed = "string",
                Color = "string",
                Description = "string",
                Microchip = "string",
                Sex = "string",
                Fixed = true,
                Weight = 0,
                Birthday = DateTime.UtcNow,
                AdoptionDay = DateTime.UtcNow,
                VetRecords = new FileLink
                    {
                        DbPath = "string"
                    },
                PetPhoto = new FileLink
                    {
                        DbPath = "string"
                    },
                Prescriptions = prescriptions,
                Vaccines = vaccines,
                Conditions = conditions,
                DateAdded = DateTime.UtcNow,
                DateModified = DateTime.UtcNow,
                User_id = user_id
            };
    }

    public Dto.Pet NewDtoPet()
    {
        Dto.Prescription[] prescriptions = new Dto.Prescription[]
        {
            new Dto.Prescription
            {
                Name = "string",
                Doctor = "string",
                Due = DateTime.UtcNow,
                Refills = 0
            }
        };
        Dto.Vaccine[] vaccines = new Dto.Vaccine[]
        {
            new Dto.Vaccine
            {
                Name = "string",
                DateAdministered = DateTime.UtcNow,
                DueDate = DateTime.UtcNow
            }
        };

        Dto.Condition[] conditions = new Dto.Condition[]
        {
            new Dto.Condition
            {
                Name = "string",
                Notes = "string"
            }
        };

        return new Dto.Pet
            {
                Name = "string",
                Breed = "string",
                Color = "string",
                Description = "string",
                Microchip = "string",
                Sex = "string",
                Fixed = true,
                Weight = 0,
                Birthday = DateTime.UtcNow,
                AdoptionDay = DateTime.UtcNow,
                VetRecords = new Dto.FileLink
                    {
                        DbPath = "string"
                    },
                PetPhoto = new Dto.FileLink
                    {
                        DbPath = "string"
                    },
                Prescriptions = prescriptions,
                Vaccines = vaccines,
                Conditions = conditions
            };
    }

    [Fact]
    public async void GetAll_Returns_All_Of_A_Users_Pets_As_PetListItems()
    {
        //Arrange
        var context = CreateContext();
        var petService = new PetService(context, _mapper);
        var currentCount = context.Pets.Where(p => p.User_id == user_id).Count();
        //Act
        var pets = petService.GetAll(apiVersion, user_id);
        //Assert
        pets.Length.Should().Be(currentCount);
        pets.Should().BeOfType<Dto.PetListItem[]>();
    }

    [Fact]
    public void Get_Returns_Correct_Pet()
    {
        //Arrange
        var context = CreateContext();
        var petService = new PetService(context, _mapper);
        var id = 1;
        //Act
        var pet = petService.Get(apiVersion, id);
        //Assert
        pet.Id.Should().Be(id);
    }

    [Fact]
    public void Get_Throws_PetNotFoundException_When_Pet_Does_Not_Exist()
    {
        //Arrange
        var context = CreateContext();
        var petService = new PetService(context, _mapper);
        //Act
        Action act = () => petService.Get(apiVersion, -1);
        //Assert
        act.Should().Throw<PetNotFoundException>();
    }

    [Fact]
    public void Add_Creates_A_Pet()
    {
        //Arrange
        var newPet = NewDtoPet();
        var context = CreateContext();
        var petService = new PetService(context, _mapper);
        var originalCount = context.Pets.Count();
        //Act
        petService.Add(apiVersion, user_id, newPet);
        //Assert
        context.Pets.Count().Should().Be(originalCount + 1);
        context.Conditions.Count().Should().Be(originalCount + 1);
        context.Vaccines.Count().Should().Be(originalCount + 1);
        context.Prescriptions.Count().Should().Be(originalCount + 1);
        context.FileLinks.Count().Should().Be(originalCount * 2 + 2);
    }

    [Fact]
    public void Delete_Removes_A_Pet()
    {
        //Arrange
        var context = CreateContext();
        var petService = new PetService(context, _mapper);
        int id = 1;
        var pet = context.Pets.SingleOrDefault(p => p.Id == id);
        //Act
        petService.Delete(id);
        //Assert
        context.Pets.SingleOrDefault(p => p.Id == id).Should().BeNull();
        context.Conditions.SingleOrDefault(c => c.Id == pet.Conditions[0].Id).Should().BeNull();
        context.Vaccines.SingleOrDefault(v => v.Id == pet.Vaccines[0].Id).Should().BeNull();
        context.Prescriptions.SingleOrDefault(p => p.Id == pet.Prescriptions[0].Id).Should().BeNull();
    }

    [Fact]
    public void Delete_Throws_PetNotFoundException_When_Pet_Does_Not_Exist()
    {
        //Arrange
        var context = CreateContext();
        var petService = new PetService(context, _mapper);
        //Act
        Action act = () => petService.Delete(-1);
        //Assert
        act.Should().Throw<PetNotFoundException>();
    }

    [Fact]
    public void Update_Updates_Correct_Pet()
    {
        //Arrange
        var updatedPet = NewDtoPet();
        updatedPet.Id = 2;
        updatedPet.Name = "New Name!";
        updatedPet.Conditions[0].Name = "New Condition Name!";
        var context = CreateContext();
        var petService = new PetService(context, _mapper);
        //Act
        var pet = petService.Update(apiVersion, updatedPet.Id, updatedPet);
        //Assert
        pet.Name.Should().Be(updatedPet.Name);
        pet.Conditions[0].Name.Should().Be(updatedPet.Conditions[0].Name);
    }

    [Fact]
    public void Update_Throws_PetNotFoundException_When_Pet_Does_Not_Exist()
    {
        //Arrange
        var updatedPet = NewDtoPet();
        updatedPet.Id = -1;
        var context = CreateContext();
        var petService = new PetService(context, _mapper);
        //Act
        Action act = () => petService.Update(apiVersion, -1, updatedPet);
        //Assert
        act.Should().Throw<PetNotFoundException>();
    }

    [Fact]
    public void Update_Throws_Exception_When_Pet_Id_Does_Not_Match_Parameter_Id()
    {
        //Arrange
        var updatedPet = NewDtoPet();
        updatedPet.Id = 1;
        var context = CreateContext();
        var petService = new PetService(context, _mapper);
        //Act
        Action act = () => petService.Update(apiVersion, 4, updatedPet);
        //Assert
        act.Should().Throw<Exception>();
    }
}