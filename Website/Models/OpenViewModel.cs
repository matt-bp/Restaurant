namespace Website.Models;

using Lib.Models;

public class OpenViewModel
{
    public DateOnly Date { get; set; }
    public TimeOnly Time { get; set; }
    public ICollection<Restaurant> OpenRestaurants { get; set; }
}
