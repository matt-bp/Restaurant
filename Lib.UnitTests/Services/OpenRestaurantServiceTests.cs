using Lib.Models;

namespace Lib.UnitTests.Services;

using Lib.Services;

public class OpenRestaurantServiceTests
{
    private static IEnumerable<Restaurant> MakeRestaurantList()
    {
        return new List<Restaurant>
        {
            new()
            {
                Name = "Bob's",
                Availabilities = new List<Availability>
                {
                    new()
                    {
                        Day = DayOfWeek.Monday,
                        Open = TimeOnly.Parse("09:00"),
                        Close = TimeOnly.Parse("20:00")
                    }
                }
            }
        };
    }

    [Test]
    public void GetOpenRestaurants_WhenDateCorrectAndTimeInRange_ReturnsSomeRestaurants()
    {
        var openRestaurantService = new OpenRestaurantService(MakeRestaurantList());
        
        var time = TimeOnly.Parse("10:00");

        var restaurants = openRestaurantService.GetOpenRestaurants(DayOfWeek.Monday, time);
        
        Assert.That(restaurants, Is.Not.Empty);
    }

    [Test]
    public void GetOpenRestaurants_WhenDateCorrectAndTimeNotInRange_ReturnsNoRestaurants()
    {
        var openRestaurantService = new OpenRestaurantService(MakeRestaurantList());
        var time = TimeOnly.Parse("8:00");

        var restaurants = openRestaurantService.GetOpenRestaurants(DayOfWeek.Monday, time);
        
        Assert.That(restaurants, Is.Empty);
    }
}
