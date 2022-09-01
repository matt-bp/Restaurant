namespace Lib.Models;

public class Availability 
{
    public DayOfWeek Day { get; set; }
    public TimeOnly Open { get; set; }
    public TimeOnly Close { get; set; }
}

