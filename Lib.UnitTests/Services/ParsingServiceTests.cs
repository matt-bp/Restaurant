using Lib.Models;
using Lib.Services;
using FluentAssertions;

namespace Lib.UnitTests.Services;

public class ParsingServiceTests
{
    [Test]
    public void MakeAvailabilitiesFromStr_WhenOneDayAndTimeRange_ReturnsThatOneRestaurant()
    {
        const string testString = "Mon 11:30 am - 9 pm";
        var expected = new Availability
        {
            Day = DayOfWeek.Monday,
            Open = TimeOnly.Parse("11:30"),
            Close = TimeOnly.Parse("21:00")
        };
        var result = ParsingService.MakeAvailabilitiesFromStr(testString);

        var first = result.First();
        first.Should().BeEquivalentTo(expected);
    }
}