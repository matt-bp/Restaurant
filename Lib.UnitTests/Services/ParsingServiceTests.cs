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

    [Test]
    public void MakeAvailabilitiesFromStr_WhenMonToThu_ReturnsDaysInBetween()
    {
        const string testString = "Mon-Thu 11:30 am - 9 pm";
        var expected = new List<DayOfWeek>
        {
            DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday
        };

        var result = ParsingService.MakeAvailabilitiesFromStr(testString)
            .Select(a => a.Day);
        
        Assert.That(result, Is.EquivalentTo(expected));
    }

    [Test]
    public void MakeAvailabilitiesFromStr_WhenMonToSun_ReturnsDaysInBetween()
    {
        const string testString = "Mon-Sun 11:30 am - 9 pm";
        var expected = new List<DayOfWeek>
        {
            DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday
        };

        var result = ParsingService.MakeAvailabilitiesFromStr(testString)
            .Select(a => a.Day);
        
        Assert.That(result, Is.EquivalentTo(expected));
    }

    [Test]
    public void MakeAvailabilitiesFromStr_WhenCommaSeparatedBeforeRange_ReturnsDisjointDays()
    {
        const string testString = "Mon, Wed-Fri 11:30 am - 9 pm";
        var expected = new List<DayOfWeek>
        {
            DayOfWeek.Monday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday
        };

        var result = ParsingService.MakeAvailabilitiesFromStr(testString)
            .Select(a => a.Day);

        Assert.That(result, Is.EquivalentTo(expected));
    }
    
    [Test]
    public void MakeAvailabilitiesFromStr_WhenCommaSeparatedAfterRange_ReturnsDisjointDays()
    {
        const string testString = "Mon-Wed, Sun 11:30 am - 9 pm";
        var expected = new List<DayOfWeek>
        {
            DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Sunday
        };

        var result = ParsingService.MakeAvailabilitiesFromStr(testString)
            .Select(a => a.Day);

        Assert.That(result, Is.EquivalentTo(expected));
    }
    
    [Test]
    public void MakeAvailabilitiesFromStr_WhenTwoRanges_ReturnsCorrectDays()
    {
        const string testString = "Mon-Wed, Fri-Sun 11:30 am - 9 pm";
        var expected = new List<DayOfWeek>
        {
            DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday
        };

        var result = ParsingService.MakeAvailabilitiesFromStr(testString)
            .Select(a => a.Day);

        Assert.That(result, Is.EquivalentTo(expected));
    }

    [Test]
    public void MakeAvailabilitiesFromStr_WhenBothTimesInAfternoon_ReturnsCorrectTimes()
    {
        const string testString = "Mon 1 pm - 9 pm";
        var expected = new Availability
        {
            Day = DayOfWeek.Monday,
            Open = TimeOnly.Parse("13:00"),
            Close = TimeOnly.Parse("21:00")
        };
        var result = ParsingService.MakeAvailabilitiesFromStr(testString);

        var first = result.First();
        first.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void MakeAvailabilitiesFromStr_WhenParsingNoon_ReturnsTheCorrectTime()
    {
        const string testString = "Mon 12 pm - 10 pm";
        var expected = new Availability
        {
            Day = DayOfWeek.Monday,
            Open = TimeOnly.Parse("12:00"),
            Close = TimeOnly.Parse("22:00")
        };
        var result = ParsingService.MakeAvailabilitiesFromStr(testString);

        var first = result.First();
        first.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void MakeAvailabilitiesFromStr_WhenParsingMidnight_ReturnsTheCorrectTime()
    {
        const string testString = "Mon 12 am - 10 am";
        var expected = new Availability
        {
            Day = DayOfWeek.Monday,
            Open = TimeOnly.Parse("00:00"),
            Close = TimeOnly.Parse("10:00")
        };
        var result = ParsingService.MakeAvailabilitiesFromStr(testString);

        var first = result.First();
        first.Should().BeEquivalentTo(expected);
    }
}