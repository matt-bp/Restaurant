namespace Lib.Models;

public class Restaurant
{
    public string Name { get; set; }
    public ICollection<Availability> Availabilities { get; set; }
}
