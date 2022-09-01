namespace Lib.Services;

using Models;

public interface IOpenRestaurantService
{
    public IEnumerable<Restaurant> GetOpenRestaurants(DayOfWeek date, TimeOnly time);
}

public class OpenRestaurantService : IOpenRestaurantService
{
    private IEnumerable<Restaurant> PossibleRestaurants { get; set; }

    public OpenRestaurantService(IEnumerable<Restaurant> restaurants)
    {
        var temp = restaurants.ToList();
        var first = restaurants.First().Availabilities.ToList();
        PossibleRestaurants = restaurants;
    }
    
    public IEnumerable<Restaurant> GetOpenRestaurants(DayOfWeek dayOfWeek, TimeOnly time)
    {
        return (
            from restaurant in PossibleRestaurants 
            let available = restaurant.Availabilities.Any(a => dayOfWeek == a.Day && time >= a.Open && time <= a.Close) 
            where available select restaurant
        ).ToList();
    }
}
