namespace bootcamp_api.Tests.XUnit.Services;

public class LinkServiceTests
{
    [Fact]
    public void GeneratePetsLink_Uses_ApiVersion_And_Id_To_Generate_Link()
    {
        //Arrange
        ApiVersion version = new ApiVersion(7, 0);
        int id = 20;
        //Act
        var res = LinkService.GeneratePetsLink(version, id);
        //Assert
        res.Should().Be($"/api/v{version}/Pets/{id}");
    }

    [Fact]
    public void GenerateBookmarksLink_Uses_ApiVersion_Id_And_PetfinderVersion_To_Generate_Link()
    {
        //Arrange
        ApiVersion version = new ApiVersion(7, 0);
        int id = 20;
        int pfVersion = 4;
        //Act
        var res = LinkService.GenerateBookmarksLink(version, id, pfVersion);
        //Assert
        res.Should().Be($"/api/v{version}/Bookmarks/{id}/Petfinder/v{pfVersion}");
    }

    [Fact]
    public void GeneratePetfinderLink_Uses_PetfinderVersion_And_PetfinderId_To_Generate_Link()
    {
        //Arrange
        int pfVersion = 20;
        int pfId = 4;
        //Act
        var res = LinkService.GeneratePetfinderLink(pfVersion, pfId);
        //Assert
        res.Should().Be($"/v{pfVersion}/animals/{pfId}");
    }

}