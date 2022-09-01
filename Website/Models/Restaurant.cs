namespace Website.Models;

public class Restaurant
{
    public string Name { get; set; }
    public ICollection<Availability> Availabilities { get; set; }
    
    public class Availability 
    {
        public DayOfWeek Day { get; set; }
        public TimeOnly Open { get; set; }
        public TimeOnly Close { get; set; }
    }
}
