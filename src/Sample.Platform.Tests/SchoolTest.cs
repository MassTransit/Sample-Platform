using Sample.Platform.Tests.DSL;

namespace Sample.Platform.Tests;

public class ProductSalesTest
{
    private SchoolService _SchoolService;

    public ProductSalesTest()
    {
        _SchoolService = new SchoolService();
    }

    [Fact]
    public async void CheckAdminAccount()
    {
        var school = await _SchoolService.GivenWeHaveASchool("Holy St Trinity");
        await _SchoolService.AndWeHaveAdministrator(school.SchoolId, "Alexandra");
        await _SchoolService.AndWeHaveAdministrator(school.SchoolId, "Bob");

        // TODO: Create Consumer to search using the name and then Send a Greeting based on name found
        _SchoolService.WhenSearchedNameIs(school.SchoolId, "Alex");
        _SchoolService.ThenOutputStandardGreeting();
        Assert.True(true);
    }
}
