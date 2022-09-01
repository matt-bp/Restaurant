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
        // Mon 11:30 am - 9 pm
        
        var split = strAvailability.Split(' ').Where(s => s != " ").ToArray();

        var days = new List<DayOfWeek>();
        TimeOnly? open = null;
        TimeOnly? close = null;

        var digitPattern = @"\d";
        var currentSubString = 0;

        foreach (var str in split)
        {
            if (Regex.IsMatch(str, digitPattern))
            {
                break;
            }
            
            switch (str)
            {
                case "Mon":
                    days.Add(DayOfWeek.Monday);
                    break;
            }

            currentSubString += 1;
        }


        var startTime = split[currentSubString];

        if (Regex.IsMatch(startTime, digitPattern))
        {
            var startTimeInMorning = split[currentSubString + 1] == "am";

            open = TimeOnly.Parse(startTime);
            
            var dash = split[currentSubString + 2] == "-";

            var endTime = split[currentSubString + 3];
            if (!endTime.Contains(':'))
            {
                endTime += ":00";
            }
            if (Regex.IsMatch(endTime, digitPattern))
            {
                close = TimeOnly.Parse(endTime);
                var endTimeInMorning = split[currentSubString + 4] == "am";
                if (!endTimeInMorning)
                {
                    close = close.Value.AddHours(12);
                }
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
}