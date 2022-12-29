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
    public async void CheckUserIsNotAdmin()
    {
        var school = await _SchoolService.GivenWeHaveASchool("Holy St Trinity");
        await _SchoolService.AndWeHaveAdministrator(school.SchoolId, "Alexandra");
        await _SchoolService.AndWeHaveAdministrator(school.SchoolId, "Bob");

        _SchoolService.WhenSearchedNameIs(school.SchoolId, "Alex");
        Assert.True(await _SchoolService.ThenOutputStandardGreeting());
        Assert.False(await _SchoolService.ThenOutputSpecialGreeting());
    }

    [Fact]
    public async void CheckUserIsAdmin()
    {
        var school = await _SchoolService.GivenWeHaveASchool("Holy St Trinity");
        await _SchoolService.AndWeHaveAdministrator(school.SchoolId, "Alexandra");
        await _SchoolService.AndWeHaveAdministrator(school.SchoolId, "Bob");
        await _SchoolService.AndWeHaveAdministrator(school.SchoolId, "John");

        _SchoolService.WhenSearchedNameIs(school.SchoolId, "Bob");
        Assert.False(await _SchoolService.ThenOutputStandardGreeting());
        Assert.True(await _SchoolService.ThenOutputSpecialGreeting());
    }
}
