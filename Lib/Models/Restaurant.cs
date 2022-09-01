namespace Lib.Models;

public class Restaurant
{
    public string Name { get; set; }
    public IEnumerable<Availability> Availabilities { get; set; } = Enumerable.Empty<Availability>();
}
