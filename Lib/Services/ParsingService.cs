namespace Lib.Services;

using Models;
using System.Text.RegularExpressions;

public class ParsingService
{
    public static IEnumerable<Restaurant> MakeRestaurantsFromIntermediateJson(IEnumerable<IntermediateJson> objects)
    {
        return objects.Select(o => new Restaurant
        {
            Name = o.Name,
            Availabilities = o.Times.SelectMany(MakeAvailabilitiesFromStr)
        });
    }

    public static IEnumerable<Availability> MakeAvailabilitiesFromStr(string strAvailability)
    {       
        var split = strAvailability.Split(' ').Where(s => s != " ").ToArray();
        
        TimeOnly? open = null;
        TimeOnly? close = null;

        var digitPattern = @"\d";
        
        var days = ParseDaysOfWeek(split, out var currentSubString);
        
        var startTime = AddColonIfSingleDigit(split[currentSubString]);
        if (Regex.IsMatch(startTime, digitPattern))
        {
            open = TimeOnly.Parse(startTime);
            
            var startTimeInMorning = split[currentSubString + 1] == "am";
            open = HandleTwelveHourClock(open.Value, startTimeInMorning);

            var endTime = AddColonIfSingleDigit(split[currentSubString + 3]);
            if (Regex.IsMatch(endTime, digitPattern))
            {
                close = TimeOnly.Parse(endTime);

                var endTimeInMorning = split[currentSubString + 4] == "am";
                close = HandleTwelveHourClock(close.Value, endTimeInMorning);
            }
        }

        if (open == null || close == null)
        {
            return Enumerable.Empty<Availability>();
        }

        var availabilities = days.Select(d => new Availability
        {
            Day = d,
            Open = open.Value,
            Close = close.Value
        });

        return availabilities;
    }

    private static TimeOnly HandleTwelveHourClock(TimeOnly time, bool isMorning)
    {
        if (isMorning && time.Hour != 12)
        {
            return time;
        }
        else if (!isMorning && time.Hour != 12)
        {
            return time.AddHours(12);
        }
        else if (isMorning && time.Hour == 12)
        {
            return time.AddHours(-12);
        }
        else
        {
            return time;
        }
    }

    private static string AddColonIfSingleDigit(string time)
    {
        return time.Contains(':') ? time : time + ":00";
    }

    private static IEnumerable<DayOfWeek> ParseDaysOfWeek(string[] split, out int currentIndex)
    {
        currentIndex = 0;
        
        List<DayOfWeek> days = new List<DayOfWeek>();
        
        var digitPattern = @"\d";
        
        while (!Regex.IsMatch(split[currentIndex], digitPattern))
        {
            var current = split[currentIndex];
            if (current.Contains('-'))
            {
                var splitRange = current.Split("-");
                var start = GetDayFromString(splitRange[0]);
                var end = GetDayFromString(splitRange[1]);
                days.AddRange(CreateDayRange(start, end));
            }
            else
            {
                days.Add(GetDayFromString(current));
            }
            
            currentIndex += 1;
        }
        
        return days;
    }

    private static string SanatizeDay(string day)
    {
        return day.Replace(",", "");
    }

    private static DayOfWeek GetDayFromString(string day) => SanatizeDay(day) switch
    {
        "Mon" => DayOfWeek.Monday,
        "Tue" => DayOfWeek.Tuesday,
        "Wed" => DayOfWeek.Wednesday,
        "Thu" => DayOfWeek.Thursday,
        "Fri" => DayOfWeek.Friday,
        "Sat" => DayOfWeek.Saturday,
        "Sun" => DayOfWeek.Sunday,
        _ => DayOfWeek.Sunday
    };
    
    private static IEnumerable<DayOfWeek> CreateDayRange(DayOfWeek start, DayOfWeek end)
    {
        var days = new List<DayOfWeek>();

        var currentInt = (int)start;
        var endInt = (int)end;
        while (currentInt != endInt)
        {
            var test = (DayOfWeek)currentInt;
            days.Add(test);

            currentInt = (currentInt + 1) % ((int)DayOfWeek.Saturday + 1);
        }
        
        days.Add(end);

        return days;
    }
}